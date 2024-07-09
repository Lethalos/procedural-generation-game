using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInteraction : MonoBehaviour
{
    [SerializeField] GameObject cameraPivot;
    [SerializeField] Transform exitPoint;

    private PlayerInteraction currentDriver;
    private bool isOccupied = false;

    void Update()
    {
        if (isOccupied)
        {
            currentDriver.UpdatePlayerCoordinates(transform.position);

            if (Input.GetKeyDown(KeyCode.Q))
            {
                ExitCar();
            }
        }
    }

    public void EnterCar(PlayerInteraction player)
    {
        if (!isOccupied)
        {
            isOccupied = true;
            cameraPivot.SetActive(true);
            VehiclesManager.Instance.SelectVehicleWithGO(this);
            //FindObjectOfType<TerrainGenerator>().viewer = transform;

            currentDriver = player;
            player.gameObject.SetActive(false);
        }
    }

    //private void ToggleVehicleControllers()
    //{
    //    if(isOccupied)
    //    {
    //        GetComponent<VehicleController>().enabled = true;
    //        GetComponent<TurretController>().enabled = true;
    //        GetComponent<GunsController>().enabled = true;
    //        cameraOrbit.SetActive(true);
            
    //    }
    //    else
    //    {
    //        GetComponent<VehicleController>().enabled = false;
    //        GetComponent<TurretController>().enabled = false;
    //        GetComponent<GunsController>().enabled = false;
    //        cameraOrbit.SetActive(false);
    //    }
    //}

    private void ExitCar()
    {
        if (isOccupied)
        {
            isOccupied = false;
            cameraPivot.SetActive(false);
            VehiclesManager.Instance.DeSelectVehicleWithGO(this);

            currentDriver.ExitCar(exitPoint.position);
            currentDriver = null;
        }
    }
}
