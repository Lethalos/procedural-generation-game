using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationManager : Singleton<VegetationManager>
{
    public Transform grassPrefab;
    public Transform treePrefab;

    public Transform GetGrassPrefab() { return grassPrefab; }

    public Transform GetTreePrefab() { return treePrefab; }
}
