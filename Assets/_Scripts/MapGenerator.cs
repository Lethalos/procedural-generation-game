using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] int mapWidth;
    [SerializeField] int mapHeight;
    [SerializeField] int seed;
    [SerializeField] float noiseScale;
    [SerializeField] int octaves;
    [Range(0f, 1f)]
    [SerializeField] float persistance;
    [SerializeField] float lacunarity;
    [SerializeField] Vector2 offset;

    public bool autoUpdate;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        MapDisplay mapDisplay = gameObject.GetComponent<MapDisplay>();
        mapDisplay.DrawNoiseMap(noiseMap);
    }

    private void OnValidate()
    {
        if(mapWidth < 1) mapWidth = 1;
        if(mapHeight < 1) mapHeight = 1;
        if(lacunarity < 1) lacunarity = 1;
        if(octaves < 0) octaves = 0;
    }
}
