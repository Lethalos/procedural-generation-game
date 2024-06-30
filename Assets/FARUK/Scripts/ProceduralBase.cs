using UnityEngine;
using System.Collections.Generic;

public class ProceduralBase : PersistentSingleton<ProceduralBase>
{
    public BaseObjectManager objectManager; // Reference to the ObjectManager script

    private Camera cam;
    private int distance;
    private int count = 0;

    Vector3[] vectors = new Vector3[]
    {
        new Vector3(40, 0f, 40f),
        new Vector3(40f, 0f, 15f),
        new Vector3(40f, 0f, -15f),
        new Vector3(40f, 0f, -40f),
        new Vector3(-40f, 0f, 40f),
        new Vector3(-40f, 0f, 10f),
        new Vector3(-40f, 0f, -15f),
        new Vector3(-40f, 0f, -40f),
        new Vector3(15f, 0f, 40f),
        new Vector3(15f, 0f, -40f),
        new Vector3(-15f, 0f, 40f),
        new Vector3(-15f, 0f, -40f)
    };

    private int[] indexes = new int[12];
    private int angle;
    private float angradian;
    private Vector3[] majors = new Vector3[5];
    private int[] slider_vals = new int[5];

    public GameObject barr;
    public GameObject tower;
    private GameObject road;
    private int distanceamt;

    private void Start()
    {
        // Instantiate Major objects
        //InstantiateMajor(objectManager.OC);
        //InstantiateMajor(objectManager.Arsenal);
        // Add instantiation for other Major objects as needed
        cam = Camera.main;
        road = objectManager.Road;
    }

    public void BaseBuild(Vector3 buildPos)
    {
        distanceamt = 60;

        /*
        distance = Random.Range(10 + distanceamt, 20 + distanceamt);
        */

        /*
        slider_vals[0] = (int)slider_Arsenal.value;
        slider_vals[1] = (int)slider_Barack.value;
        slider_vals[2] = (int)slider_Comissary.value;
        slider_vals[3] = (int)slider_Prison.value;
        slider_vals[4] = (int)slider_Storage.value;
        */


        //Base ayarlarý ayarlama menüsü olmadýðý zaman hepsi 60
        slider_vals[0] = 0;
        slider_vals[1] = 0;
        slider_vals[2] = 0;
        slider_vals[3] = 0;
        slider_vals[4] = 0;

        distance = -40;


        // mousePos.y = build_0.transform.localScale.y / 2;
        //Instantiate(objectManager.OC, buildPos, Quaternion.identity);

        AssignVectorsDist(buildPos);

        //Debug.Log("after" + majors[0]);

        InstantiateMajor(objectManager.OC, buildPos);
        InstantiateMajor(objectManager.Arsenal, majors[0]);
        InstantiateMajor(objectManager.Barrack, majors[1]);
        InstantiateMajor(objectManager.Commissary, majors[2]);
        InstantiateMajor(objectManager.Prison, majors[3]);
        InstantiateMajor(objectManager.Storage, majors[4]);

        for (int f = 0; f < 5; f++)
        {
            RoadConnectMajor(buildPos, majors[f], buildPos);
        }


        for (int s = 0; s < 5; s++)
        {
            RoadConnectMajor(buildPos, majors[s], buildPos);
        }

        for (int k = 0; k < 5; k++)
        {
            if (k == 4)
            {
                RoadConnectMajor(majors[k], majors[0], buildPos);
                break;
            }
            RoadConnectMajor(majors[k], majors[k + 1], buildPos);

        }

        //wallss(buildPos, dist);
        Defence(buildPos, distance);
    }

    bool IsItused(int a)
    {
        for (int i = 0; i < 12; i++)
        {
            if (indexes[i] == default(int))
            {
                indexes[i] = a; return false;
            }
            else if (indexes[i] == a) return true;

        }
        return false;
    }

    //private void AssignVectors(Vector3 baseloc)
    //{
    //    angle = Random.Range(0, 360);
    //    for (int i = 0; i < 5; i++)
    //    {
    //        angradian = angle * 0.0174533f;
    //        majors[i] = new Vector3((60 + distance) * Mathf.Cos(angradian) + baseloc.x, 0f, (60 + distance) * Mathf.Sin(angradian) + baseloc.z);
    //        angle = angle + 67;
    //    }
    //}

    private void AssignVectorsDist(Vector3 baseloc)
    {
        angle = Random.Range(0, 360);
        for (int i = 0; i < 5; i++)
        {
            angradian = angle * 0.0174533f;
            majors[i] = new Vector3((60 + slider_vals[i]) * Mathf.Cos(angradian) + baseloc.x, 0f, (60 + slider_vals[i]) * Mathf.Sin(angradian) + baseloc.z);
            angle = angle + 67;
        }
    }

    //private void RoadConnect(Vector3 startpt, Vector3 endpt)
    //{
    //    float x, z;
    //    int numofroads_x, numofroads_z;
    //    int signx = 1;
    //    int signz = 1;
    //    x = endpt.x - startpt.x;
    //    z = endpt.z - startpt.z;
    //    Vector3 road_vec = startpt;
    //    numofroads_x = (int)x / 10;
    //    numofroads_z = (int)z / 10;
    //    //Debug.Log("number of roads needed in X " + numofroads_x);
    //    //Debug.Log("number of roads needed in Z " + numofroads_x);

    //    if (Mathf.Abs(numofroads_x) > Mathf.Abs(numofroads_z))
    //    {
    //        if (numofroads_x < 0) { signx = -1; }
    //        if (numofroads_z < 0) { signz = -1; }
    //        for (int i = 0; i < Mathf.Abs(numofroads_x); i++)
    //        {
    //            road_vec.x = road_vec.x + 10f * signx;
    //            Instantiate(road, road_vec, Quaternion.Euler(-90, 90, 90));
    //        }
    //        for (int j = 0; j < Mathf.Abs(numofroads_z); j++)
    //        {
    //            road_vec.z = road_vec.z + 10f * signz;
    //            Instantiate(road, road_vec, Quaternion.Euler(-90, 0, 90));
    //        }
    //    }
    //    else
    //    {
    //        if (numofroads_x < 0) { signx = -1; }
    //        if (numofroads_z < 0) { signz = -1; }

    //        for (int i = 0; i < Mathf.Abs(numofroads_z); i++)
    //        {
    //            road_vec.z = road_vec.z + 10f * signz;
    //            Instantiate(road, road_vec, Quaternion.Euler(-90, 0, 90));
    //        }

    //        for (int j = 0; j < Mathf.Abs(numofroads_x); j++)
    //        {
    //            road_vec.x = road_vec.x + 10f * signx;
    //            Instantiate(road, road_vec, Quaternion.Euler(-90, 90, 90));
    //        }
    //    }

    //}

    private void RoadConnectMajor(Vector3 startpt, Vector3 endpt, Vector3 basek)
    {
        float x, z;
        int numofroads_x, numofroads_z;
        int signx = 1;
        int signz = 1;
        x = endpt.x - startpt.x;
        z = endpt.z - startpt.z;
        Vector3 road_vec = startpt;
        numofroads_x = (int)x / 10;
        numofroads_z = (int)z / 10;
        //Debug.Log("number of roads needed in X " + numofroads_x);
        //Debug.Log("number of roads needed in Z " + numofroads_x);

        if (Mathf.Abs(basek.x - road_vec.x) > Mathf.Abs(basek.x - (road_vec.x + 10f * signx)))
        {
            if (numofroads_x < 0) { signx = -1; }
            if (numofroads_z < 0) { signz = -1; }
            for (int i = 0; i < Mathf.Abs(numofroads_x); i++)
            {
                road_vec.x = road_vec.x + 10f * signx;
                if (TryGetTerrainHeight(road_vec, out Vector3 newPosition))
                {
                    Instantiate(road, newPosition, Quaternion.Euler(0, 90, 0));
                }
            }
            for (int j = 0; j < Mathf.Abs(numofroads_z); j++)
            {
                road_vec.z = road_vec.z + 10f * signz;
                if (TryGetTerrainHeight(road_vec, out Vector3 newPosition))
                {
                    Instantiate(road, newPosition, Quaternion.Euler(0, 0, 0));
                }
            }
        }
        else
        {

            if (numofroads_x < 0) { signx = -1; }
            if (numofroads_z < 0) { signz = -1; }
            for (int i = 0; i < Mathf.Abs(numofroads_z); i++)
            {
                road_vec.z = road_vec.z + 10f * signz;
                if (TryGetTerrainHeight(road_vec, out Vector3 newPosition))
                {
                    Instantiate(road, newPosition, Quaternion.Euler(0, 0, 0));
                }
            }
            for (int j = 0; j < Mathf.Abs(numofroads_x); j++)
            {
                road_vec.x = road_vec.x + 10f * signx;
                if (TryGetTerrainHeight(road_vec, out Vector3 newPosition))
                {
                    Instantiate(road, newPosition, Quaternion.Euler(0, 90, 0));
                }
            }
        }

    }

    //private void Roads(Vector3 Center)
    //{

    //    Vector3 roads_ver = Center;
    //    Vector3 roads_hor = Center;
    //    roads_ver.x = roads_ver.x - 15f;
    //    roads_hor.z = roads_hor.z - 15f;

    //    Instantiate(road, roads_ver, Quaternion.Euler(0, 90, 0));
    //    Instantiate(road, roads_hor, Quaternion.Euler(0, 0, 0));
    //    roads_ver.x = roads_ver.x - 10f;
    //    roads_hor.z = roads_hor.z - 10f;

    //    Instantiate(road, roads_ver, Quaternion.Euler(0, 90, 0));
    //    Instantiate(road, roads_hor, Quaternion.Euler(0, 0, 0));
    //    roads_ver.x = roads_ver.x + 40f;
    //    roads_hor.z = roads_hor.z + 40f;

    //    Instantiate(road, roads_ver, Quaternion.Euler(0, 90, 0));
    //    Instantiate(road, roads_hor, Quaternion.Euler(0, 0, 0));
    //    roads_ver.x = roads_ver.x + 10f;
    //    roads_hor.z = roads_hor.z + 10f;

    //    Instantiate(road, roads_ver, Quaternion.Euler(0, 90, 0));
    //    Instantiate(road, roads_hor, Quaternion.Euler(0, 0, 0));


    //    Vector3 roads_lb = Center;
    //    Vector3 roads_rb = Center;
    //    Vector3 roads_lt = Center;
    //    Vector3 roads_rt = Center;

    //    //left bottom
    //    roads_lb.x = roads_lb.x - 30f;
    //    roads_lb.z = roads_lb.z - 25f;

    //    //right bottom
    //    roads_rb.x = roads_rb.x + 30f;
    //    roads_rb.z = roads_rb.z - 25f;

    //    //left top
    //    roads_lt.x = roads_lt.x - 25f;
    //    roads_lt.z = roads_lt.z + 30f;

    //    //right top
    //    roads_rt.x = roads_rt.x - 25f;
    //    roads_rt.z = roads_rt.z - 30f;

    //    for (int i = 0; i < 6; i++)
    //    {
    //        Instantiate(road, roads_lb, Quaternion.Euler(0, 0, 0));
    //        Instantiate(road, roads_rb, Quaternion.Euler(0, 0, 0));
    //        Instantiate(road, roads_lt, Quaternion.Euler(0, 90, 0));
    //        Instantiate(road, roads_rt, Quaternion.Euler(0, 90, 0));

    //        roads_lb.z = roads_lb.z + 10f;
    //        roads_rb.z = roads_rb.z + 10f;
    //        roads_lt.x = roads_lt.x + 10f;
    //        roads_rt.x = roads_rt.x + 10f;
    //    }
    //}

    private void Defence(Vector3 basek, int dist)
    {
        Vector3 temp = basek;
        Vector3 periphery_start;
        Vector3 periphery_end;
        temp.x = temp.x + 115 + dist;
        temp.z = temp.z + 120 + dist;

        periphery_start = temp;
        periphery_start.z = periphery_start.z - 20;
        periphery_start.x = periphery_start.x - 5;
        periphery_end = periphery_start;
        periphery_end.x = periphery_end.x - 220 - dist * 2;
        RoadConnectMajor(periphery_start, periphery_end, basek);
        periphery_start = periphery_end;
        periphery_start.z = periphery_start.z - 230 - dist * 2;
        RoadConnectMajor(periphery_start, periphery_end, basek);
        periphery_start.z = periphery_start.z + 10;
        periphery_end = periphery_start;
        periphery_end.x = periphery_end.x + 210 + dist * 2;
        RoadConnectMajor(periphery_start, periphery_end, basek);
        periphery_start = periphery_end;
        periphery_start.z = periphery_start.z + 220 + dist * 2;
        RoadConnectMajor(periphery_start, periphery_end, basek);

        if (TryGetTerrainHeight(temp, out Vector3 newPosition))
        {
            Instantiate(barr, newPosition, Quaternion.Euler(0, -90, 0));
        }

        for (int i = 0; i < (120 + dist) / 5; i++)
        {
            temp.x = temp.x - 10f;
            if (i == (120 + dist) / 10 || i == (120 + dist) / 10 + 1 || i == (120 + dist) / 10 - 1)
            {
                RoadConnectMajor(basek, temp, basek);
                continue;
            }

            if (TryGetTerrainHeight(temp, out newPosition))
            {
                Instantiate(barr, newPosition, Quaternion.Euler(0, -90, 0));
            }

            if (i % 5 == 0 && i != 0 && i < (120 + dist) / 5 - 4)
            {
                if (TryGetTerrainHeight(temp, out newPosition))
                {
                    Instantiate(tower, newPosition, Quaternion.identity);
                }
            }
        }

        temp.x = temp.x - 5f;

        if (TryGetTerrainHeight(temp, out newPosition))
        {
            Instantiate(tower, newPosition, Quaternion.identity);
        }

        temp.z = temp.z - 5f;

        if (TryGetTerrainHeight(temp, out newPosition))
        {
            Instantiate(barr, newPosition, Quaternion.Euler(0, 180, 0));
        }

        for (int i = 0; i < (120 + dist) / 5; i++)
        {
            temp.z = temp.z - 10f;

            if (TryGetTerrainHeight(temp, out newPosition))
            {
                Instantiate(barr, newPosition, Quaternion.Euler(0, 180, 0));
            }

            if (i % 5 == 0 && i != 0 && i < (120 + dist) / 5 - 4)
            {
                if (TryGetTerrainHeight(temp, out newPosition))
                {
                    Instantiate(tower, newPosition, Quaternion.identity);
                }
            }
        }

        temp.z = temp.z - 5f;

        if (TryGetTerrainHeight(temp, out newPosition))
        {
            Instantiate(tower, newPosition, Quaternion.identity);
        }

        temp.x = temp.x + 5f;

        if (TryGetTerrainHeight(temp, out newPosition))
        {
            Instantiate(barr, newPosition, Quaternion.Euler(0, 90, 0));
        }

        for (int i = 0; i < (120 + dist) / 5; i++)
        {
            temp.x = temp.x + 10f;

            if (TryGetTerrainHeight(temp, out newPosition))
            {
                Instantiate(barr, newPosition, Quaternion.Euler(0, 90, 0));
            }

            if (i % 5 == 0 && i != 0 && i < (120 + dist) / 5 - 4)
            {
                if (TryGetTerrainHeight(temp, out newPosition))
                {
                    Instantiate(tower, newPosition, Quaternion.identity);
                }
            }
        }

        temp.x = temp.x + 5f;

        if (TryGetTerrainHeight(temp, out newPosition))
        {
            Instantiate(tower, newPosition, Quaternion.identity);
        }

        temp.z = temp.z + 5f;

        if (TryGetTerrainHeight(temp, out newPosition))
        {
            Instantiate(barr, newPosition, Quaternion.identity);
        }

        for (int i = 0; i < (120 + dist) / 5; i++)
        {
            temp.z = temp.z + 10f;

            if (TryGetTerrainHeight(temp, out newPosition))
            {
                Instantiate(barr, newPosition, Quaternion.identity);
            }

            if (i % 5 == 0 && i != 0 && i < (120 + dist) / 5 - 4)
            {
                if (TryGetTerrainHeight(temp, out newPosition))
                {
                    Instantiate(tower, newPosition, Quaternion.identity);
                }
            }
        }

        temp.z = temp.z + 5f;

        if (TryGetTerrainHeight(temp, out newPosition))
        {
            Instantiate(tower, newPosition, Quaternion.identity);
        }
    }

    private Vector3 Calcul(Vector3 input)
    {
        input.x = input.x + 10;

        return input;
    }

    // Function to instantiate Major objects and their connected Minor objects
    private void InstantiateMajor(GameObject majorObj, Vector3 place)
    {

        Vector3 minorplace = new Vector3(0f, 0f, 0f);

        if (TryGetTerrainHeight(place, out Vector3 newPosition))
        {
            Instantiate(majorObj, newPosition, Quaternion.identity);
        }

        // Access Minor objects connected to the Major object
        List<GameObject> connectedMinors = objectManager.GetConnectedMinors(majorObj);
        /*
        // Instantiate the connected Minor objects
        foreach (GameObject minorObj in connectedMinors)
        {
            Instantiate(minorObj,Calcul(place), Quaternion.identity, instantiatedMajor.transform);
        }*/
        for (int i = 0; i < connectedMinors.Count; i++)
        {

            int randomIndex = Random.Range(0, 12);

            for (int j = 0; j < 12; j++)
            {
                if (IsItused(randomIndex) == false)
                {
                    minorplace.x = vectors[randomIndex].x;
                    minorplace.y = vectors[randomIndex].y;
                    minorplace.z = vectors[randomIndex].z;
                    break;
                }
                else
                {
                    randomIndex = Random.Range(0, 12);
                    continue;
                }
            }

            // Instantiate the current Minor object as a child of the Major object
            // Instantiate(connectedMinors[i], place + minorplace, Quaternion.identity, instantiatedMajor.transform);
            // RoadConnectMajor(place, place + minorplace, place);
        }

        for (int k = 0; k < indexes.Length; k++)
        {
            indexes[k] = default(int);
        }

        for (int l = 0; l < connectedMinors.Count; l++)
        {

        }
    }

    private bool TryGetTerrainHeight(Vector3 position, out Vector3 newPosition)
    {
        Ray ray = new Ray(position + Vector3.up * 100f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 200f, LayerMask.GetMask("Building")))
        {
            newPosition = position;
            return false;
        }

        if (Physics.Raycast(ray, out hit, 200f, LayerMask.GetMask("Terrain")))
        {
            newPosition = new Vector3(position.x, hit.point.y, position.z);
            return true;
        }

        newPosition = position;

        return false;
    }

    //private Vector3 PlaceOnGround(Vector3 position)
    //{
    //    Debug.DrawRay(position + Vector3.up * 100f, Vector3.down * 200f, Color.red, 60f);

    //    Ray ray = new Ray(position + Vector3.up * 100f, Vector3.down);
    //    if (Physics.Raycast(ray, out RaycastHit hit, 200f, LayerMask.GetMask("Terrain")))
    //    {
    //        position.y = hit.point.y;
    //    }

    //    return position;
    //}
}