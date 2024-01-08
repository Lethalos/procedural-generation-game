using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationSpawner : MonoBehaviour
{
    public GameObject grassPrefab;
    public GameObject shrubPrefab;
    // Add more vegetation prefabs as needed

    public int terrainWidth = 100;
    public int terrainLength = 100;
    public int spacing = 5; // Distance between vegetation instances

    void Start()
    {
        //PlaceVegetation();
    }

    void PlaceVegetation()
    {
        for (int x = 0; x < terrainWidth; x += spacing)
        {
            for (int z = 0; z < terrainLength; z += spacing)
            {
                Vector3 position = new Vector3(x, 0, z);
                position.y = GetTerrainHeight(position);
                Vector3 normal = GetTerrainNormal(position);
                float slope = CalculateSlope(normal);

                GameObject vegetationPrefab = ChooseVegetationPrefab(slope);
                if (vegetationPrefab != null)
                {
                    // Randomize rotation and slight scale variation
                    Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    float scaleVariation = Random.Range(0.8f, 1.2f);
                    Vector3 scale = new Vector3(scaleVariation, scaleVariation, scaleVariation);

                    GameObject vegetation = Instantiate(vegetationPrefab, position, rotation);
                    vegetation.transform.localScale = scale;
                }
            }
        }
    }

    float CalculateSlope(Vector3 normal)
    {
        return Vector3.Angle(normal, Vector3.up);
    }

    GameObject ChooseVegetationPrefab(float slope)
    {
        if (slope < 20) return grassPrefab;
        else if (slope < 40) return shrubPrefab;
        else return null; // Too steep for vegetation
    }

    float GetTerrainHeight(Vector3 position)
    {
        // Implement this method based on your terrain's data structure
        return 0; // Placeholder
    }

    Vector3 GetTerrainNormal(Vector3 position)
    {
        // Implement this method based on your terrain's data structure
        return Vector3.up; // Placeholder
    }
}
