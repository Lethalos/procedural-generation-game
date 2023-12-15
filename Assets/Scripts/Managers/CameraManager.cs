using System;
using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float zoomInFactor = 0.1f; // The amount to zoom in the camera by
    public float zoomSpeed = 5f; // The speed of the camera zoom

    private Camera mainCamera;
    private float originalFieldOfView;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool isZoomed = false; // Flag to track zoomed state
    public event Action ZoomOutEvent;
    public event Action<GameObject> ZoomInEvent;
    private void Start()
    {
        mainCamera = Camera.main;
        originalFieldOfView = mainCamera.fieldOfView; // Store the original field of view of the camera
        originalCameraPosition = mainCamera.transform.position; // Store the original sampleCenter of the camera



        originalCameraRotation = mainCamera.transform.rotation;

       // ZoomOutEvent += PoolManager.Instance.DeactivateSecondAndThirdObjects;
       // ZoomInEvent += PoolManager.Instance.ActivateSameTypeObjects;
       
       
    }

    public void ZoomIn(GameObject target)
    {
        if (!isZoomed)
        {
            if (target == null || target.transform == mainCamera.transform) // Check if the target is null or if it's the camera itself
                return;

            StopAllCoroutines();
            StartCoroutine(ZoomInCamera(target));
            isZoomed = true;
            ZoomInEvent?.Invoke(target);
        }
    }

    public void ZoomOut()
    {
        if (isZoomed)
        {
            StopAllCoroutines();
            StartCoroutine(ZoomOutCamera());
            isZoomed = false;
            ZoomOutEvent?.Invoke();
        }
    }

    public IEnumerator ZoomInCamera(GameObject target)
    {
        Vector3 targetPosition = target.transform.position; // Position of the target object

        float zoomInDistance = Vector3.Distance(mainCamera.transform.position, targetPosition) * zoomInFactor;

        Vector3 startPosition = mainCamera.transform.position;
        Vector3 desiredPosition = targetPosition - mainCamera.transform.forward * zoomInDistance;

        float zoomInSpeed = 0.7f / zoomSpeed; // Speed for smooth interpolation

        float t = 0f; // Interpolation parameter

        while (t < 1f)
        {
            t += Time.deltaTime * zoomInSpeed;

            // Interpolate the camera's sampleCenter towards the desired sampleCenter
            mainCamera.transform.position = Vector3.Lerp(startPosition, desiredPosition, t);

            // Keep the camera's rotation unchanged
            mainCamera.transform.LookAt(targetPosition);

            yield return null;
        }

        mainCamera.transform.position = desiredPosition;
        mainCamera.transform.LookAt(targetPosition);
    }






    public IEnumerator ZoomOutCamera()
    {
        Vector3 startPosition = mainCamera.transform.position;
        Vector3 desiredPosition = originalCameraPosition;

        Quaternion startRotation = mainCamera.transform.rotation;
        Quaternion desiredRotation = originalCameraRotation;

        float zoomOutSpeed = 0.7f / zoomSpeed; // Speed for smooth interpolation

        float t = 0f; // Interpolation parameter

        while (t < 1f)
        {
            t += Time.deltaTime * zoomOutSpeed;

            // Interpolate the camera's sampleCenter towards the desired sampleCenter
            mainCamera.transform.position = Vector3.Lerp(startPosition, desiredPosition, t);

            // Smoothly interpolate the camera's field of view towards the original value
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, originalFieldOfView, t);

            // Interpolate the camera's rotation towards the desired rotation
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, desiredRotation, t);

            yield return null;
        }

        mainCamera.transform.position = desiredPosition;
        mainCamera.fieldOfView = originalFieldOfView;
        mainCamera.transform.rotation = desiredRotation;
        mainCamera.transform.LookAt(desiredPosition);
    }







    public bool IsZoomedIn()
    {
        return isZoomed;
    }
}
