using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationInstancer : MonoBehaviour
{
    private GameObject grassPrefab;
    private int grassInstanceCount = 2000;
    private int treeInstanceCount = 100;
    private Vector3 areaSize = new Vector3(100, 0, 100);
    private Vector3 chunkCenter;
    private int currentLODIndex;
    private int terrainLayer;
    private float maxSlope = 20f;

    private List<Matrix4x4[]> grassMatrices;
    private List<(Matrix4x4[], GameObject)> treeMatrices; // Tuple to store matrices and corresponding prefab
    private MaterialPropertyBlock propertyBlock;
    private Vector3[] raycastStartPositions;

    private bool isInitialized = false;

    private void Update()
    {
        if (!isInitialized) return;

        RenderVegetation(grassPrefab, grassMatrices);
        RenderTrees();
    }

    void RenderVegetation(GameObject prefab, List<Matrix4x4[]> matricesList)
    {
        MeshFilter[] meshFilters = prefab.GetComponentsInChildren<MeshFilter>();
        MeshRenderer[] meshRenderers = prefab.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < meshFilters.Length; i++)
        {
            Mesh mesh = meshFilters[i].sharedMesh;
            Material material = meshRenderers[i].sharedMaterial;

            if (mesh != null && material != null)
            {
                foreach (var matrices in matricesList)
                {
                    Graphics.DrawMeshInstanced(mesh, 0, material, matrices, matrices.Length, propertyBlock);
                }
            }
        }
    }

    public void RenderGrass()
    {
        RenderVegetation(grassPrefab, grassMatrices);
    }

    public void RenderTrees()
    {
        foreach (var treeData in treeMatrices)
        {
            GameObject prefab = treeData.Item2;
            List<Matrix4x4[]> matricesList = new List<Matrix4x4[]> { treeData.Item1 };

            RenderVegetation(prefab, matricesList);
        }
    }

    public void InitializeVegetation(Vector3 areaSize, Vector3 chunkCenter, int currentLODIndex, int terrainLayer)
    {
        if (isInitialized) return;

        grassPrefab = VegetationManager.Instance.grassPrefab;
        this.areaSize = areaSize;
        this.chunkCenter = chunkCenter;
        this.currentLODIndex = currentLODIndex;
        this.terrainLayer = terrainLayer;

        propertyBlock = new MaterialPropertyBlock();
        raycastStartPositions = new Vector3[grassInstanceCount + treeInstanceCount];

        grassMatrices = new List<Matrix4x4[]>();
        treeMatrices = new List<(Matrix4x4[], GameObject)>();

        StartCoroutine(InitializeGrass());
        StartCoroutine(InitializeTrees());

        isInitialized = true;
    }

    IEnumerator InitializeGrass()
    {
        Matrix4x4[] matrices = new Matrix4x4[grassInstanceCount];

        for (int i = 0; i < grassInstanceCount; i++)
        {
            Vector3 position = new Vector3(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                0,
                Random.Range(-areaSize.z / 2, areaSize.z / 2)
            ) + chunkCenter;

            raycastStartPositions[i] = new Vector3(position.x, 1000f, position.z);

            if (TryGetTerrainHeight(position, out Vector3 terrainPosition, out Vector3 normal))
            {
                float slope = Vector3.Angle(Vector3.up, normal);

                if (slope <= maxSlope)
                {
                    float scale = Random.Range(0.5f, 0.7f);
                    Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0); // Random rotation around Y-axis
                    matrices[i] = Matrix4x4.TRS(terrainPosition, rotation, new Vector3(scale * 15, scale * 5, scale * 15));
                }
                else
                {
                    matrices[i] = Matrix4x4.zero;
                }
            }
            else
            {
                matrices[i] = Matrix4x4.zero;
            }

            if (i % 100 == 0)
            {
                yield return null;
            }
        }

        grassMatrices.Add(matrices);
    }

    IEnumerator InitializeTrees()
    {
        for (int i = 0; i < treeInstanceCount; i++)
        {
            GameObject treePrefab = VegetationManager.Instance.treePrefabs[Random.Range(0, VegetationManager.Instance.treePrefabs.Count)];
            Matrix4x4[] matrices = new Matrix4x4[1]; // Single matrix per tree for simplicity

            Vector3 position = new Vector3(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                0,
                Random.Range(-areaSize.z / 2, areaSize.z / 2)
            ) + chunkCenter;

            raycastStartPositions[grassInstanceCount + i] = new Vector3(position.x, 1000f, position.z);

            if (TryGetTerrainHeight(position, out Vector3 terrainPosition, out Vector3 normal))
            {
                float slope = Vector3.Angle(Vector3.up, normal);

                if (slope <= maxSlope)
                {
                    float scale = Random.Range(1f, 1.5f);
                    Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0); // Random rotation around Y-axis
                    matrices[0] = Matrix4x4.TRS(terrainPosition, rotation, new Vector3(scale, scale, scale));
                }
                else
                {
                    matrices[0] = Matrix4x4.zero;
                }
            }
            else
            {
                matrices[0] = Matrix4x4.zero;
            }

            treeMatrices.Add((matrices, treePrefab));

            if (i % 10 == 0)
            {
                yield return null;
            }
        }
    }

    bool TryGetTerrainHeight(Vector3 position, out Vector3 terrainPosition, out Vector3 normal)
    {
        RaycastHit hit;
        int layerMask = 1 << terrainLayer;

        if (Physics.Raycast(new Vector3(position.x, 1000f, position.z), Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Building")))
        {
            terrainPosition = position;
            normal = Vector3.up;
            return false;
        }

        if (Physics.Raycast(new Vector3(position.x, 1000f, position.z), Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            terrainPosition = hit.point;
            normal = hit.normal;
            return true;
        }

        terrainPosition = position;
        normal = Vector3.up;
        return false;
    }
}
