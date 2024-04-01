using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationSpawner : MonoBehaviour
{
    public int spacing = 5; // Distance between vegetation instances

    void Start()
    {
        
    }

    public void PlaceVegetation(float terrainWidth, float terrainLength)
    {
        for (int x = 0; x < terrainWidth; x += spacing)
        {
            for (int z = 0; z < terrainLength; z += spacing)
            {
                Vector3 position = new Vector3(x, 0, z);
                position.y = GetTerrainHeight(position);
                Vector3 normal = GetTerrainNormal(position);
                float slope = CalculateSlope(normal);

                Transform vegetationPrefab = ChooseVegetationPrefab(slope);
                if (vegetationPrefab != null)
                {
                    // Randomize rotation and slight scale variation
                    Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    float scaleVariation = Random.Range(0.8f, 1.2f);
                    Vector3 scale = new Vector3(scaleVariation, scaleVariation, scaleVariation);

                    Transform vegetation = Instantiate(vegetationPrefab, transform.parent);
                    vegetation.localScale = scale;
                }
            }
        }
    }

    float CalculateSlope(Vector3 normal)
    {
        return Vector3.Angle(normal, Vector3.up);
    }

    private Transform ChooseVegetationPrefab(float slope)
    {
        if (slope < 20) return VegetationManager.Instance.treePrefab;
        else if (slope < 40) return VegetationManager.Instance.grassPrefab;
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
