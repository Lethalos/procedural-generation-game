using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager>
{
    [SerializeField] Transform headquarterPrefab;

    public void Generate(Vector3 centerPos, Transform parent)
    {
        Instantiate(headquarterPrefab, centerPos, Quaternion.identity, parent);
    }
}
