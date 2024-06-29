using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicOC : MonoBehaviour
{
    public Camera mainCamera;
    public float checkInterval = 0.1f; // Time interval between checks
    private float timer;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            PerformCulling();
            timer = 0;
        }
    }

    void PerformCulling()
    {
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        foreach (Renderer renderer in renderers)
        {
            if (GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
            {
                // Object is within the camera's view frustum
                renderer.enabled = true;
            }
            else
            {
                // Object is outside the camera's view frustum
                renderer.enabled = false;
            }
        }
    }
}
