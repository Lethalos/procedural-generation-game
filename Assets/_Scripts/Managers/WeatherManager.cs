using DistantLands.Cozy;
using DistantLands.Cozy.Data;
using System.Collections;
using System.Collections.Generic;
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
            LoadWeatherProfile(0);
        }
        else if (Input.GetKeyDown("2"))
        {
            LoadWeatherProfile(1);
        }
        else if (Input.GetKeyDown("3"))
        {
            LoadWeatherProfile(2);
        }
        else if (Input.GetKeyDown("4"))
        {
            LoadWeatherProfile(3);
        }
        else if (Input.GetKeyDown("5"))
        {
            LoadWeatherProfile(4);
        }
        else if (Input.GetKeyDown("6"))
        {
            LoadWeatherProfile(5);
        }
        else if (Input.GetKeyDown("7"))
        {
            LoadWeatherProfile(6);
        }
        else if (Input.GetKeyDown("8"))
        {
            LoadWeatherProfile(7);
        }
        else if (Input.GetKeyDown("9"))
        {
            LoadWeatherProfile(8);
        }
    }
}
