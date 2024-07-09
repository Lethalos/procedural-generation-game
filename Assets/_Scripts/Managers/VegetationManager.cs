using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VegetationManager : PersistentSingleton<VegetationManager>
{
    public GameObject grassPrefab;
    public List<GameObject> treePrefabs;
    public List<Material> vegetationMaterials;

    public Color32 vegetationGreen = new Color32(0, 100, 0, 1);

    private void Start()
    {
        foreach (Material material in vegetationMaterials)
        {
            material.SetColor("_TopColor", vegetationGreen);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("TerrainMenu");
        }
    }

    private void OnDisable()
    {
        foreach (Material material in vegetationMaterials)
        {
            material.SetColor("_TopColor", vegetationGreen);
        }
    }

    public void UpdateAllTreeFoliageMaterials(Color32 foliageColor)
    {
        foreach (Material material in vegetationMaterials)
        {
            StartCoroutine(UpdateTreeFoliageMaterial(material, foliageColor));
        }
    }

    private IEnumerator UpdateTreeFoliageMaterial(Material material, Color32 foliageColor)
    {
        Color currentColor = material.GetColor("_TopColor");

        float startTime = 0f;
        float duration = 30f;

        while (startTime < duration)
        {
            startTime += Time.deltaTime;
            float t = startTime / duration;

            Color32 tempColor = Color32.Lerp(currentColor, foliageColor, t);
            material.SetColor("_TopColor", tempColor);
            yield return new WaitForEndOfFrame();
        }
    }
}
