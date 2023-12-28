using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Transactions;

public class FlatRegionAnalyzer : MonoBehaviour
{
    public float maxSlope = 0.03f;

    private QuadTreeNode quadTreeRoot;

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh terrainMesh = meshFilter.mesh;

        Bounds terrainBounds = terrainMesh.bounds;
        //print("Center: " + terrainBounds.center);
        //print("Size: " + terrainBounds.size);
        quadTreeRoot = new QuadTreeNode(terrainBounds, terrainMesh.vertices, maxSlope, 0);

        QuadTreeRegion flatRegion = FindFlatRegion(quadTreeRoot);

        if (flatRegion != null)
        {
            //Debug.Log("Flat Region Found: " + flatRegion.bounds);
            //CreateCube(flatRegion);

            Vector3 buildingPos =  transform.TransformPoint(flatRegion.bounds.center);
            BuildingManager.Instance.Generate(new Vector3(buildingPos.x, 0f, buildingPos.z), transform);
        }
        else
        {
            //Debug.Log("No Flat Region Found");
        }
    }

    public void CreateCube(QuadTreeRegion flatRegion)
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

    void OnDrawGizmos()
    {
        if (quadTreeRoot != null)
        {
            DrawNodeGizmos(quadTreeRoot);
        }
    }

    void DrawNodeGizmos(QuadTreeNode node, int row = 0)
    {
        if (row == 0)
            Gizmos.color = Color.green;
        if (row == 1)
            Gizmos.color = Color.red;
        if (row == 2)
            Gizmos.color = Color.yellow;
        if (row == 3)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(transform.TransformPoint(node.bounds.center), node.bounds.size);
        }

        if (!node.IsLeafNode())
        {
            foreach (var child in node.children)
            {
                DrawNodeGizmos(child, row + 1);
            }
        }
    }
}

public class QuadTreeNode
{
    public Bounds bounds;
    public Vector3[] vertices;
    public float maxSlope;
    public QuadTreeNode[] children;

    public static int MaxDepth = 3;
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






