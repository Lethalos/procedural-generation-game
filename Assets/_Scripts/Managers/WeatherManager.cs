using DistantLands.Cozy;
using DistantLands.Cozy.Data;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [SerializeField] WeatherProfile[] weatherProfiles;

    private void Start()
    {
        CozyWeather.instance.weatherModule.ecosystem.SetWeather(weatherProfiles[0]);
    }

    private void Update()
    {
        ChangeWeatherFromKeyboard();
    }

    private void LoadWeatherProfile(int index)
    {
        if (index >= 0 && index < weatherProfiles.Length)
        {
            CozyWeather.instance.weatherModule.ecosystem.SetWeather(weatherProfiles[index]);

            Debug.Log("Loaded ScriptableObject: " + weatherProfiles[index].name);
        }
        else
        {
            Debug.LogError("Invalid index specified.");
        }
    }

    public void ChangeWeatherFromKeyboard()
    {
        if (Input.GetKeyDown("1"))
        {
            VegetationManager.Instance.UpdateAllTreeFoliageMaterials(VegetationManager.Instance.vegetationGreen);
            LoadWeatherProfile(0);
        }
        else if (Input.GetKeyDown("2"))
        {
            VegetationManager.Instance.UpdateAllTreeFoliageMaterials(Color.white);
            LoadWeatherProfile(1);
        }
        else if (Input.GetKeyDown("3"))
        {
            VegetationManager.Instance.UpdateAllTreeFoliageMaterials(Color.white);
            LoadWeatherProfile(2);
        }
        else if (Input.GetKeyDown("4"))
        {
            VegetationManager.Instance.UpdateAllTreeFoliageMaterials(VegetationManager.Instance.vegetationGreen);
            LoadWeatherProfile(3);
        }
        else if (Input.GetKeyDown("5"))
        {
            VegetationManager.Instance.UpdateAllTreeFoliageMaterials(VegetationManager.Instance.vegetationGreen);
            LoadWeatherProfile(4);
        }
        else if (Input.GetKeyDown("6"))
        {
            VegetationManager.Instance.UpdateAllTreeFoliageMaterials(VegetationManager.Instance.vegetationGreen);
            LoadWeatherProfile(5);
        }
        else if (Input.GetKeyDown("7"))
        {
            VegetationManager.Instance.UpdateAllTreeFoliageMaterials(VegetationManager.Instance.vegetationGreen);
            LoadWeatherProfile(6);
        }
        else if (Input.GetKeyDown("8"))
        {
            VegetationManager.Instance.UpdateAllTreeFoliageMaterials(VegetationManager.Instance.vegetationGreen);
            LoadWeatherProfile(7);
        }
        else if (Input.GetKeyDown("9"))
        {
            VegetationManager.Instance.UpdateAllTreeFoliageMaterials(VegetationManager.Instance.vegetationGreen);
            LoadWeatherProfile(8);
        }
    }
}
