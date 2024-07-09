using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class FlatRegionAnalyzer : MonoBehaviour
{
    [SerializeField] private float maxSlope = 0.03f;
    private int terrainLayer = 3;

    private QuadTreeNode quadTreeRoot;

    private bool isAnalyzed = false;

    void Start()
    {
        if (isAnalyzed) return;

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh terrainMesh = meshFilter.mesh;

        Bounds terrainBounds = terrainMesh.bounds;
        quadTreeRoot = new QuadTreeNode(terrainBounds, terrainMesh.vertices, maxSlope, 0);

        QuadTreeRegion flatRegion = FindFlatRegion(quadTreeRoot);

        if (flatRegion != null)
        {
            StartCoroutine(CreateBase(flatRegion));
        }

        isAnalyzed = true;
    }

    private IEnumerator CreateBase(QuadTreeRegion flatRegion)
    {
        yield return new WaitForSeconds(1.5f);
        
        if (TryGetTerrainHeight(transform.TransformPoint(flatRegion.bounds.center), out Vector3 terrainPosition, out Vector3 normal))
        {
            //Debug.Log("Building created at " + terrainPosition);
            Vector3 buildingPos = terrainPosition;
            //BuildingManager.Instance.Generate(new Vector3(buildingPos.x, 0f, buildingPos.z), transform);

            ProceduralBase.Instance.BaseBuild(new Vector3(buildingPos.x, 0f, buildingPos.z), transform);
        }
    }

    private bool TryGetTerrainHeight(Vector3 position, out Vector3 terrainPosition, out Vector3 normal)
    {
        RaycastHit hit;
        int layerMask = 1 << terrainLayer;

        Vector3 rayOrigin = new Vector3(position.x, 1000f, position.z);
        Vector3 rayDirection = Vector3.down;

        // Draw the ray in the Scene view for debugging
        Debug.DrawRay(rayOrigin, rayDirection * 1000f, Color.red, 15f);

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, Mathf.Infinity, layerMask))
        {
            terrainPosition = hit.point;
            normal = hit.normal;
            return true;
        }

        terrainPosition = position;
        normal = Vector3.up;
        return false;
    }

    //private void CreatePit(QuadTreeRegion flatRegion)
    //{
    //    MeshFilter meshFilter = GetComponent<MeshFilter>();
    //    Mesh terrainMesh = meshFilter.mesh;
    //    Vector3[] vertices = terrainMesh.vertices;

    //    Bounds flatBounds = flatRegion.bounds;
    //    float maxDepth = -15.0f; // Maximum depth of the pit at its center
    //    Vector3 center = flatBounds.center;

    //    // Calculate the radius of the flat region
    //    float radius = Mathf.Max(flatBounds.size.x, flatBounds.size.z) / 2;

    //    for (int i = 0; i < vertices.Length; i++)
    //    {
    //        if (flatBounds.Contains(vertices[i]))
    //        {
    //            // Calculate distance from the center of the flat region
    //            float distance = Vector3.Distance(new Vector3(vertices[i].x, 0, vertices[i].z), new Vector3(center.x, 0, center.z));

    //            // Normalize distance based on the radius of the flat region
    //            float normalizedDistance = distance / radius;

    //            // Ensure normalized distance does not exceed 1
    //            normalizedDistance = Mathf.Min(normalizedDistance, 1);

    //            // Calculate depth based on the normalized distance (creates a bowl shape)
    //            float depth = maxDepth * Mathf.Cos(normalizedDistance * Mathf.PI / 2);

    //            // Apply the calculated depth
    //            vertices[i].y += depth;
    //        }
    //    }

    //    // Update the mesh with the new vertices and recalculate normals
    //    terrainMesh.vertices = vertices;
    //    terrainMesh.RecalculateNormals();
    //    meshFilter.mesh = terrainMesh;

    //    Debug.Log("Mesh updated!");

    //    // Update the mesh collider if needed
    //    UpdateMeshCollider(terrainMesh);
    //}

    //private void UpdateMeshCollider(Mesh terrainMesh)
    //{
    //    MeshCollider meshCollider = GetComponent<MeshCollider>();
    //    if (meshCollider != null)
    //    {
    //        Debug.Log("Updating mesh collider");
    //        meshCollider.sharedMesh = null;
    //        meshCollider.sharedMesh = terrainMesh;
    //    }
    //}

    private void CreateCube(QuadTreeRegion flatRegion)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = transform;

        cube.transform.position = transform.TransformPoint(flatRegion.bounds.center);
        cube.transform.localRotation = Quaternion.identity;
        cube.transform.localScale = flatRegion.bounds.size;
    }

    QuadTreeRegion FindFlatRegion(QuadTreeNode node)
    {
        if (node.IsLeafNode())
        {
            if (node.IsFlat())
            {
                return new QuadTreeRegion(node.bounds);
            }
            else
            {
                return null;
            }
        }
        else
        {
            foreach (QuadTreeNode child in node.children)
            {
                QuadTreeRegion flatRegion = FindFlatRegion(child);
                if (flatRegion != null)
                {
                    return flatRegion;
                }
            }
            return null;
        }
    }

    //void OnDrawGizmos()
    //{
    //    if (quadTreeRoot != null)
    //    {
    //        DrawNodeGizmos(quadTreeRoot);
    //    }
    //}

    //void DrawNodeGizmos(QuadTreeNode node, int row = 0)
    //{
    //    if (row == 0)
    //    {
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawWireCube(transform.TransformPoint(node.bounds.center), node.bounds.size);
    //    }
    //    if (row == 1)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawWireCube(transform.TransformPoint(node.bounds.center), node.bounds.size);
    //    }
    //    if (row == 2)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawWireCube(transform.TransformPoint(node.bounds.center), node.bounds.size);
    //    }
    //    if (row == 3)
    //    {
    //        Gizmos.color = Color.black;
    //        Gizmos.DrawWireCube(transform.TransformPoint(node.bounds.center), node.bounds.size);
    //    }

    //    if (!node.IsLeafNode())
    //    {
    //        foreach (var child in node.children)
    //        {
    //            DrawNodeGizmos(child, row + 1);
    //        }
    //    }
    //}
}

public class QuadTreeNode
{
    public Bounds bounds;
    public Vector3[] vertices;
    public float maxSlope;
    public QuadTreeNode[] children;

    public static int MaxDepth = 0;
    private int currentDepth = 0;

    public QuadTreeNode(Bounds bounds, Vector3[] vertices, float maxSlope, int depth)
    {
        this.bounds = bounds;
        this.vertices = vertices;
        this.maxSlope = maxSlope;
        this.children = new QuadTreeNode[4];
        this.currentDepth = depth;

        Subdivide();
    }

    void Subdivide()
    {
        if (currentDepth < MaxDepth)
        {
            Vector3 center = bounds.center;
            Vector3 size = bounds.size;

            float halfWidth = size.x * 0.5f;
            float halfLength = size.z * 0.5f;

            Bounds[] subBounds = new Bounds[4];
            subBounds[0] = new Bounds(center + new Vector3(-halfWidth / 2f, 0, -halfLength / 2f), new Vector3(halfWidth, size.y, halfLength));
            subBounds[1] = new Bounds(center + new Vector3(halfWidth / 2f, 0, halfLength / 2f), new Vector3(halfWidth, size.y, halfLength));
            subBounds[2] = new Bounds(center + new Vector3(-halfWidth / 2f, 0, halfLength / 2f), new Vector3(halfWidth, size.y, halfLength));
            subBounds[3] = new Bounds(center + new Vector3(halfWidth / 2f, 0, -halfLength / 2f), new Vector3(halfWidth, size.y, halfLength));

            for (int i = 0; i < 4; i++)
            {
                // Performans için Linq yerine basit döngü kullanýmý
                List<Vector3> childVerticesList = new List<Vector3>();
                foreach (var v in vertices)
                {
                    if (subBounds[i].Contains(v))
                        childVerticesList.Add(v);
                }
                Vector3[] childVertices = childVerticesList.ToArray();

                //int numberOfVerticesInBounds = CountVerticesInExpandedBounds(subBounds[i], childVertices, 100f);
                //Debug.Log("Number of vertices in bounds: " + numberOfVerticesInBounds);

                children[i] = new QuadTreeNode(subBounds[i], childVertices, maxSlope, currentDepth + 1);
            }
        }
    }

    public int CountVerticesInExpandedBounds(Bounds bounds, Vector3[] vertices, float expandBy)
    {
        int count = 0;
        Bounds expandedBounds = new Bounds(bounds.center, bounds.size + new Vector3(expandBy, expandBy, expandBy));

        foreach (Vector3 vertex in vertices)
        {
            if (expandedBounds.Contains(vertex))
            {
                count++;
            }
        }
        return count;
    }

    public bool IsLeafNode()
    {
        return children[0] == null;
    }

    public bool IsFlat()
    {
        if (vertices.Length == 0)
            return false;

        // Calculate the average height of the vertices
        float averageHeight = vertices.Average(v => v.y);

        // Determine the maximum deviation from the average height
        float maxDeviation = vertices.Max(v => Mathf.Abs(v.y - averageHeight));

        // Compare the max deviation with a threshold derived from maxSlope and bounds size
        float threshold = maxSlope * bounds.size.magnitude;

        // If the maximum deviation is less than or equal to the threshold, the area is considered flat
        return maxDeviation <= threshold;
    }
}

public class QuadTreeRegion
{
    public Bounds bounds;

    public QuadTreeRegion(Bounds bounds)
    {
        this.bounds = bounds;
    }
}






