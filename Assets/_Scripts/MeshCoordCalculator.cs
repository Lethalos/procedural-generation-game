using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeshCoordCalculator : MonoBehaviour
{
    [SerializeField] float delayBetweenVertices = 0.1f;
    [SerializeField] float yAxisAddValue = 10f;

    void Start()
    {
        StartCoroutine(ModifyVerticesWithDelay());
        //ModifyVertices();
    }

    IEnumerator ModifyVerticesWithDelay()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter != null)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].y += yAxisAddValue;
                mesh.vertices = vertices;

                print("Vertex " + i + " " + vertices[i]);

                //yield return new WaitForSeconds(delayBetweenVertices);
                yield return new WaitForEndOfFrame();
            }

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
        else
        {
            Debug.LogError("MeshFilter component not found!");
        }
    }
    private void ModifyVertices()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (meshFilter != null)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].z += yAxisAddValue;
                mesh.vertices = vertices;

                print("Vertex " + i + " " + vertices[i]);
            }

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
        else
        {
            Debug.LogError("MeshFilter component not found!");
        }
    }
}

