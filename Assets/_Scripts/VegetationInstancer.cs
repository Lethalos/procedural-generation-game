using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationInstancer : MonoBehaviour
{
    public GameObject grassPrefab;
    public GameObject treePrefab;
    public int grassInstanceCount = 500;
    public int treeInstanceCount = 100;
    public Vector3 areaSize = new Vector3(100, 0, 100);
    public Vector3 chunkCenter;
    public int currentLODIndex;
    public int terrainLayer;
    private float maxSlope = 20f;

    private List<Matrix4x4[]> grassMatrices;
    private List<Matrix4x4[]> treeMatrices;
    private MaterialPropertyBlock propertyBlock;
    private Vector3[] raycastStartPositions;

    void Start()
    {
        InitializeVegetation();
    }

    void Update()
    {
        if (currentLODIndex != 0)
        {
            Destroy(this);
            return;
        }

        RenderVegetation(grassPrefab, grassMatrices);
        RenderVegetation(treePrefab, treeMatrices);
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

    void InitializeVegetation()
    {
        propertyBlock = new MaterialPropertyBlock();
        raycastStartPositions = new Vector3[grassInstanceCount + treeInstanceCount];

        grassMatrices = new List<Matrix4x4[]>();
        treeMatrices = new List<Matrix4x4[]>();

        StartCoroutine(InitializeGrass());
        StartCoroutine(InitializeTrees());
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
                    float scale = Random.Range(5f, 7f);
                    matrices[i] = Matrix4x4.TRS(terrainPosition, Quaternion.identity, new Vector3(scale, scale, scale));
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
        Matrix4x4[] matrices = new Matrix4x4[treeInstanceCount];

        for (int i = 0; i < treeInstanceCount; i++)
        {
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
                    float scale = Random.Range(2.0f, 3.0f);
                    matrices[i] = Matrix4x4.TRS(terrainPosition, Quaternion.identity, new Vector3(scale, scale, scale));
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

        treeMatrices.Add(matrices);
    }

    bool TryGetTerrainHeight(Vector3 position, out Vector3 terrainPosition, out Vector3 normal)
    {
        RaycastHit hit;
        int layerMask = 1 << terrainLayer;

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
