using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclesManager : PersistentSingleton<VehiclesManager>
{
    public VehicleCamera cam;
    public KeyCode nextVehicle = KeyCode.C;
    public KeyCode previousVehicle = KeyCode.V;
    public GameObject[] vehicles;
    public string vehiclesTag = "Vehicles";
    public int vehicleIndex = 0;

    private void Start()
    {
        //vehicles = GameObject.FindGameObjectsWithTag(vehiclesTag);
        //vehicleIndex = Random.Range(0, vehicles.Length);
        //SelectVehicle();
    }

    private void Update()
    {
        //int previousVehicleIndex = vehicleIndex;

        //if (Input.GetKeyDown(nextVehicle))
        //{
        //    if (vehicleIndex >= vehicles.Length - 1)
        //        vehicleIndex = 0;
        //    else
        //        vehicleIndex++;
        //}

        //if (Input.GetKeyDown(previousVehicle))
        //{
        //    if (vehicleIndex <= 0)
        //        vehicleIndex = vehicles.Length -1;
        //    else
        //        vehicleIndex--;
        //}

        //if (previousVehicleIndex != vehicleIndex)
        //    SelectVehicle();
    }

    public void SelectVehicleWithGO(VehicleInteraction vehicleInteraction)
    {
        cam.target = vehicleInteraction.gameObject.GetComponent<TurretController>().cameraTarget;

        VehicleController vController = vehicleInteraction.gameObject.GetComponent<VehicleController>();

        if (vController)
            vController.controlVehicle = true;

        TanksController tController = vehicleInteraction.gameObject.GetComponent<TanksController>();

        if (tController)
            tController.controlVehicle = true;

        TurretController turretController = vehicleInteraction.gameObject.GetComponent<TurretController>();

        if(turretController)
        {
            turretController.turretActive = true;
            turretController.cam = cam.gameObject;
        }

        GunsController gunsController = vehicleInteraction.gameObject.GetComponent<GunsController>();

        if (gunsController)
            gunsController.gunsActive = true;
    }

    public void DeSelectVehicleWithGO(VehicleInteraction vehicleInteraction)
    {
        VehicleController vController = vehicleInteraction.gameObject.GetComponent<VehicleController>();

        if (vController)
            vController.controlVehicle = false;

        TanksController tController = vehicleInteraction.gameObject.GetComponent<TanksController>();

        if (tController)
            tController.controlVehicle = false;

        TurretController turretController = vehicleInteraction.gameObject.GetComponent<TurretController>();

        if (turretController)
            turretController.turretActive = false;

        GunsController gunsController = vehicleInteraction.gameObject.GetComponent<GunsController>();

        if (gunsController)
            gunsController.gunsActive = false;
    }

    void SelectVehicle()
    {
        for (int i = 0; i < vehicles.Length; i++)
        {
            if (i == vehicleIndex)
            {
                cam.target = vehicles[i].GetComponent<TurretController>().cameraTarget;

                VehicleController vController = vehicles[i].GetComponent<VehicleController>();

                if (vController)
                    vController.controlVehicle = true;

                TanksController tController = vehicles[i].GetComponent<TanksController>();

                if (tController)
                    tController.controlVehicle = true;

                FindObjectOfType<TerrainGenerator>().viewer = vehicles[i].transform;
            }
            else
            {
                VehicleController vController = vehicles[i].GetComponent<VehicleController>();

                if (vController)
                    vController.controlVehicle = false;

                TanksController tController = vehicles[i].GetComponent<TanksController>();

                if (tController)
                    tController.controlVehicle = false;
            }
        }
    }
}
