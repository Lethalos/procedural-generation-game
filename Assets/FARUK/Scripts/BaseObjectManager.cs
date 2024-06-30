using System.Collections.Generic;
using UnityEngine;

public class BaseObjectManager : MonoBehaviour
{
    // Public dictionary to store connections between Major and Minor objects
    public Dictionary<GameObject, List<GameObject>> majorToMinorMap = new Dictionary<GameObject, List<GameObject>>();

    // Public list to store all Major objects (including Base)
    public List<GameObject> majorObjects = new List<GameObject>();

    // Public list to store all Minor objects
    public List<GameObject> minorObjects = new List<GameObject>();

    // Public GameObjects for each Major object
    public GameObject OC;
    public GameObject Arsenal;
    public GameObject Barrack;
    public GameObject Commissary;
    public GameObject Prison;
    public GameObject Storage;
    public GameObject Wall;

    // Public GameObjects for each Minor object
    public GameObject Maintenance;
    public GameObject Helipad;
    public GameObject ATC;
    public GameObject Store;
    public GameObject Workshop;
    public GameObject Admin;
    public GameObject Chamber;
    public GameObject Hospital;
    public GameObject Canteen;
    public GameObject NecessityAdmin;
    public GameObject Housing;
    public GameObject Cells;
    public GameObject CanteenPrison;
    public GameObject Silo;
    public GameObject Management;
    public GameObject Depot;
    public GameObject Road;

    // Function to set up connections between Major and Minor objects
    public void SetupConnections()
    {
        // Clear existing data to avoid duplicates
        majorObjects.Clear();
        minorObjects.Clear();
        majorToMinorMap.Clear();

        // Populate major objects and minor objects
        majorObjects.Add(OC);
        majorObjects.Add(Arsenal);
        majorObjects.Add(Barrack);
        majorObjects.Add(Commissary);
        majorObjects.Add(Prison);
        majorObjects.Add(Storage);
        majorObjects.Add(Wall);
        majorObjects.Add(Road);

        minorObjects.Add(Maintenance);
        minorObjects.Add(Helipad);
        minorObjects.Add(ATC);
        minorObjects.Add(Store);
        minorObjects.Add(Workshop);
        minorObjects.Add(Admin);
        minorObjects.Add(Chamber);
        minorObjects.Add(Hospital);
        minorObjects.Add(Canteen);
        minorObjects.Add(NecessityAdmin);
        minorObjects.Add(Housing);
        minorObjects.Add(Cells);
        minorObjects.Add(CanteenPrison);
        minorObjects.Add(Silo);
        minorObjects.Add(Management);
        minorObjects.Add(Depot);

        // Set up connections between Major and Minor objects
        AddToMap(OC, new List<GameObject> { Maintenance, Helipad, ATC });
        AddToMap(Arsenal, new List<GameObject> { Store, Workshop, Admin });
        AddToMap(Barrack, new List<GameObject> { Chamber, Hospital });
        AddToMap(Commissary, new List<GameObject> { Canteen, NecessityAdmin, Housing });
        AddToMap(Prison, new List<GameObject> { Cells, CanteenPrison });
        AddToMap(Storage, new List<GameObject> { Silo, Management });
        AddToMap(Wall, new List<GameObject> { Depot });
    }

    // Helper function to add entries to the dictionary, checking for duplicates
    private void AddToMap(GameObject majorObj, List<GameObject> minorObjs)
    {
        if (!majorToMinorMap.ContainsKey(majorObj))
        {
            majorToMinorMap.Add(majorObj, minorObjs);
        }
        else
        {
            Debug.LogWarning($"Major object {majorObj.name} is already in the dictionary.");
        }
    }

    // Function to access Minor objects connected to a specific Major object
    public List<GameObject> GetConnectedMinors(GameObject majorObj)
    {
        if (majorToMinorMap.ContainsKey(majorObj))
        {
            return majorToMinorMap[majorObj];
        }
        else
        {
            Debug.LogWarning("Major object not found in the dictionary.");
            return new List<GameObject>();
        }
    }

    // Example usage
    private void Start()
    {
        SetupConnections();
        /*
        // Access Minor objects connected to the Base (OC)
        List<GameObject> ocConnectedMinors = GetConnectedMinors(OC);
        foreach (GameObject minorObj in ocConnectedMinors)
        {
            // Do something with the Minor object
        }

        // Access Minor objects connected to the Arsenal
        List<GameObject> arsenalConnectedMinors = GetConnectedMinors(Arsenal);
        foreach (GameObject minorObj in arsenalConnectedMinors)
        {
            // Do something with the Minor object
        }*/
    }
}
