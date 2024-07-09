using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 5f;
    public KeyCode interactionKey = KeyCode.E;

    private Camera playerCamera;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            TryInteractWithCar();
        }
    }

    void TryInteractWithCar()
    {
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactionDistance, Color.red,60f);

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance))
        {
            VehicleInteraction car = hit.collider.GetComponentInParent<VehicleInteraction>();
            if (car != null)
            {
                car.EnterCar(this);
            }
        }
    }

    public void UpdatePlayerCoordinates(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void ExitCar(Vector3 exitPosition)
    {
        gameObject.SetActive(true);
        transform.position = exitPosition;
        //FindObjectOfType<TerrainGenerator>().viewer = transform;
    }
}
