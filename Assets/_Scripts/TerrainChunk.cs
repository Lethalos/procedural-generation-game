using System;
using System.Timers;
using UnityEngine;

public class TerrainChunk
{
    const float colliderGenerationDistanceThreshold = 5;
    public event System.Action<TerrainChunk, bool> onVisibilityChanged;
    public Vector2 coord;

    private GameObject meshObject;
    private Vector2 sampleCenter;
    private Bounds bounds;

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    private LODInfo[] detailLevels;
    private LODMesh[] lodMeshes;
    private int colliderLODIndex;

    private HeightMap heightMap;
    private bool heightMapReceived;
    private int previousLODIndex = -1;
    private bool hasSetCollider;
    private float maxViewDst;

    private HeightMapSettings heightMapSettings;
    private MeshSettings meshSettings;
    private Transform viewer;

    private bool hasFlatRegionCollider = false;
    private Vector3 chunkWorldPosition;

    private int terrainLayer;

    public TerrainChunk(Vector2 coord, HeightMapSettings heightMapSettings, MeshSettings meshSettings, LODInfo[] detailLevels, int colliderLODIndex, Transform parent, Transform viewer, Material material, int terrainLayer)
    {
        this.coord = coord;
        this.heightMapSettings = heightMapSettings;
        this.meshSettings = meshSettings;
        this.detailLevels = detailLevels;
        this.colliderLODIndex = colliderLODIndex;
        this.viewer = viewer;
        this.terrainLayer = terrainLayer;

        sampleCenter = coord * meshSettings.MeshWorldSize / this.meshSettings.meshScale;
        Vector2 position = coord * meshSettings.MeshWorldSize;
        bounds = new Bounds(sampleCenter, Vector2.one * meshSettings.MeshWorldSize);

        chunkWorldPosition = new Vector3(position.x, 0, position.y);

        meshObject = new GameObject("Terrain Chunk");
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshCollider = meshObject.AddComponent<MeshCollider>();

        // Microsplat
        MicroSplatMeshTerrain microSplatMeshTerrain = meshObject.AddComponent<MicroSplatMeshTerrain>();
        microSplatMeshTerrain.templateMaterial = material;
        microSplatMeshTerrain.meshTerrains = new MeshRenderer[1];
        microSplatMeshTerrain.meshTerrains[0] = meshRenderer;
        meshRenderer.material = material;

        meshObject.transform.position = chunkWorldPosition;
        meshObject.transform.parent = parent;

        // Set the layer of the terrain chunk
        meshObject.layer = terrainLayer;

        SetVisible(false);

        lodMeshes = new LODMesh[detailLevels.Length];
        for (int i = 0; i < detailLevels.Length; i++)
        {
            lodMeshes[i] = new LODMesh(detailLevels[i].lod);
            lodMeshes[i].updateCallback += UpdateTerrainChunk;
            if (i == colliderLODIndex)
            {
                lodMeshes[i].updateCallback += UpdateCollisionMesh;
            }
        }

        maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
    }

    public void Load()
    {
        ThreadedDataRequester.RequestData(() => GenerateHeightMap(), OnHeightMapReceived);
    }

    private HeightMap GenerateHeightMap()
    {
        return HeightMapGenerator.GenerateHeightMap(meshSettings.NumVertsPerLine, meshSettings.NumVertsPerLine, heightMapSettings, sampleCenter);
    }

    private void OnHeightMapReceived(object heightMapObject)
    {
        this.heightMap = (HeightMap)heightMapObject;
        heightMapReceived = true;

        UpdateTerrainChunk();
    }

    Vector2 ViewerPosition => new Vector2(viewer.position.x, viewer.position.z);

    public void UpdateTerrainChunk()
    {
        if (!heightMapReceived) return;

        float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(ViewerPosition));

        //Debug.Log("Viewer distance from nearest edge: " + viewerDstFromNearestEdge + " Viewer Position " + ViewerPosition);

        bool wasVisible = IsVisible();
        bool visible = viewerDstFromNearestEdge <= maxViewDst;

        if (visible)
        {
            int lodIndex = CalculateLODIndex(viewerDstFromNearestEdge);
            if (lodIndex != previousLODIndex)
            {
                LODMesh lodMesh = lodMeshes[lodIndex];
                if (lodMesh.hasMesh)
                {
                    previousLODIndex = lodIndex;
                    meshFilter.mesh = lodMesh.mesh;
                    meshCollider.sharedMesh = null;
                    meshCollider.sharedMesh = lodMesh.mesh;

                    HandleVegetationInstancing(lodIndex);
                    HandleBuildingInstancing(lodIndex);
                }
                else if (!lodMesh.hasRequestedMesh)
                {
                    lodMesh.RequestMesh(heightMap, meshSettings);
                }
            }
        }

        if (wasVisible != visible)
        {
            SetVisible(visible);
            onVisibilityChanged?.Invoke(this, visible);
        }
    }

    private int CalculateLODIndex(float viewerDstFromNearestEdge)
    {
        int lodIndex = 0;
        for (int i = 0; i < detailLevels.Length - 1; i++)
        {
            if (viewerDstFromNearestEdge > detailLevels[i].visibleDstThreshold)
            {
                lodIndex = i + 1;
            }
            else
            {
                break;
            }
        }
        return lodIndex;
    }

    private void HandleBuildingInstancing(int lodIndex)
    {
        FlatRegionAnalyzer flatRegionAnalyzer = meshObject.GetComponent<FlatRegionAnalyzer>();

        if (lodIndex == 0)
        {
            if (flatRegionAnalyzer == null)
            {
                meshObject.AddComponent<FlatRegionAnalyzer>();
            }
            else flatRegionAnalyzer.enabled = true;
        }
        else
        {
            if (flatRegionAnalyzer != null) flatRegionAnalyzer.enabled = false;
        }
    }

    private void HandleVegetationInstancing(int lodIndex)
    {
        VegetationInstancer vegetationInstancer = meshObject.GetComponent<VegetationInstancer>();
        
        if (lodIndex == 0)
        {
            if (vegetationInstancer == null)
            {
                vegetationInstancer = meshObject.AddComponent<VegetationInstancer>();
                vegetationInstancer.InitializeVegetation(new Vector3(meshSettings.MeshWorldSize, 0f, meshSettings.MeshWorldSize), chunkWorldPosition, lodIndex, terrainLayer);
            }
            else vegetationInstancer.enabled = true;
        }
        else
        {
            if (vegetationInstancer != null) vegetationInstancer.enabled = false;
        }
    }

    public void UpdateCollisionMesh()
    {
        if (!hasSetCollider)
        {
            float sqrDistanceFromViewerToEdge = bounds.SqrDistance(ViewerPosition);
            if (sqrDistanceFromViewerToEdge < detailLevels[colliderLODIndex].sqVisibleDstThreshold)
            {
                if (!lodMeshes[colliderLODIndex].hasRequestedMesh)
                {
                    lodMeshes[colliderLODIndex].RequestMesh(heightMap, meshSettings);
                }
            }

            if (sqrDistanceFromViewerToEdge < colliderGenerationDistanceThreshold * colliderGenerationDistanceThreshold && !hasFlatRegionCollider)
            {
                if (lodMeshes[colliderLODIndex].hasMesh)
                {
                    meshCollider.sharedMesh = lodMeshes[colliderLODIndex].mesh;
                    hasSetCollider = true;
                }
            }
        }
    }

    public void SetVisible(bool visible)
    {
        meshObject.SetActive(visible);
    }

    public bool IsVisible()
    {
        return meshObject.activeSelf;
    }
}

class LODMesh
{
    public Mesh mesh;
    public bool hasRequestedMesh;
    public bool hasMesh;
    int lod;
    public event System.Action updateCallback;

    public LODMesh(int lod)
    {
        this.lod = lod;
    }

    private void OnMeshDataReceived(object meshDataObject)
    {
        mesh = ((MeshData)meshDataObject).CreateMesh();
        hasMesh = true;
        updateCallback();
    }

    public void RequestMesh(HeightMap heightMap, MeshSettings meshSettings)
    {
        hasRequestedMesh = true;
        ThreadedDataRequester.RequestData(() => MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, lod), OnMeshDataReceived);
    }
}