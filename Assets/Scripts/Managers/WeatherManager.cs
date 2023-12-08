using DG.Tweening.Core.Easing;
using DistantLands.Cozy;
using DistantLands.Cozy.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectLoader : MonoBehaviour
{
    public T[] LoadAllScriptableObjects<T>() where T : ScriptableObject
    {
        T[] loadedObjects = Resources.LoadAll<T>("");

        if (loadedObjects == null || loadedObjects.Length == 0)
        {
            Debug.LogError($"No ScriptableObjects of type {typeof(T)} found in the Resources folder.");
        }

        return loadedObjects;
    }
}
public class WeatherManager : SingletonBehaviour<WeatherManager>
{
    public WeatherProfile[] scriptableObjects;
    ScriptableObjectLoader scriptableObjectLoader;
    public CozyWeather cozyWeather;
    public CozyEcosystem cozyEcosystem;
    GameState gameState;
    [SerializeField] Light light_;
    [SerializeField] GameObject sun;
    Animation sunAnim;
    public event Action PopupEnableEvent;

    private void Start()
    {
        cozyWeather = cozyWeather.GetComponent<CozyWeather>();
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        cozyEcosystem.weatherTransitionTime = 1f;
        light_=light_.GetComponent<Light>();
        // LoadScriptableObjectAtIndex(0); 
        sunAnim = sun.GetComponent<Animation>();
        sun.SetActive(false);
    

    }
    //private void Update()
    //{
    //    ChangeWeatherFromKeyboard();
        
    //}
    private void GameManagerOnGameStateChanged(GameState state)
    {

    }

    private void LoadScriptableObjectAtIndex(int index)
    {
        if (index >= 0 && index < scriptableObjects.Length)
        {
            WeatherProfile selectedObject = scriptableObjects[index];
            selectedObject.SetWeatherWeight(1f);
            cozyWeather.SetWeather(selectedObject);
            cozyWeather.GetCurrentWeatherProfile();
           

            // Use the selected ScriptableObject as needed
            Debug.Log("Loaded ScriptableObject: " + selectedObject.name);
        }
        else
        {
            Debug.LogError("Invalid index specified.");
        }
    }
    public void ClearWeather()
    {
        LoadScriptableObjectAtIndex(0);
        //if (sun.activeInHierarchy && sunAnim.enabled ==true)
        //{


        //    WaitCoroutine(5f);
        //    Debug.Log("Delay coroutine to set active false.");
        //    sun.SetActive(false);
        //}
        if(sun.activeInHierarchy)
        {
            sunAnim.Play("Sunset");
            WaitCoroutine(2f);
            sun.SetActive(false);
        }
        UIManager.Instance.HideAllPopUps();
    }
    public void ChangeWeather(GameObject poolObject)
    {   
        UIManager.Instance.CloudPopupAnimation(poolObject);
        PoolObjectType poolObjectType = PoolManager.Instance.GetObjectTypeFromGameObject(poolObject);
        Debug.Log("Object type: "+poolObjectType.ToString());
        if(poolObjectType != PoolObjectType.None)
        {   
            foreach(PoolInfo poolInfo in PoolManager.Instance.listOfPool)
            {
                if (poolInfo.type == poolObjectType)
                {
                    if (poolObjectType == PoolObjectType.Altocumulus)
                    {
                        LoadScriptableObjectAtIndex(1);
                        light_.intensity = 6.31f;
                        poolObject.SetActive(false);
                        
                    }
                    else if (poolObjectType == PoolObjectType.Altostratus)
                    {
                        LoadScriptableObjectAtIndex(2);
                        light_.intensity = 2.69f;
                        sun.SetActive(true);
                        sunAnim.enabled = true;
                        sunAnim.Play("Sunrise");
                      
                        poolObject.SetActive(false);
                        
                    }
                    else if (poolObjectType == PoolObjectType.Cirrostratus)
                    {
                        sun.SetActive(true);
                        LoadScriptableObjectAtIndex(3);
                        light_.intensity = 5.37f;
                       
                        sunAnim.enabled = true;
                        sunAnim.Play("Sunrise");
                        poolObject.SetActive(false);
                       
                    }
                    else if (poolObjectType == PoolObjectType.Cirrucomulus)
                    {
                        LoadScriptableObjectAtIndex(4);
                        light_.intensity = 5.21f;
                        poolObject.SetActive(false);
                       
                    }
                    else if (poolObjectType == PoolObjectType.Cirrus)
                    {
                        sun.SetActive(true);
                        LoadScriptableObjectAtIndex(5);
                        light_.intensity = 5.21f;
                        sunAnim.enabled = true;
                       
                        sunAnim.Play("Sunrise");
                        poolObject.SetActive(false);
                       
                    }
                    else if (poolObjectType == PoolObjectType.Cumulonimbus)
                    {
                        LoadScriptableObjectAtIndex(6);
                        light_.intensity = 2.72f;
                        poolObject.SetActive(false);
                      
                    }
                    else if (poolObjectType == PoolObjectType.Cumulus)
                    {
                        sun.SetActive(true);
                        LoadScriptableObjectAtIndex(7);
                        light_.intensity = 5.21f;
                        sunAnim.enabled=true;
                       
                        sunAnim.Play("Sunrise");
                        poolObject.SetActive(false);
                      
                    }
                    else if (poolObjectType == PoolObjectType.Stratcumulus)
                    {
                        LoadScriptableObjectAtIndex(8);
                        poolObject.SetActive(false);
                       

                    }
                    else if (poolObjectType == PoolObjectType.Strutus)
                    {   
                        LoadScriptableObjectAtIndex(9);
                        light_.intensity = 2.72f;
                        poolObject.SetActive(false);
                       
                    }
                    else if(poolObjectType== PoolObjectType.None)
                    {
                        LoadScriptableObjectAtIndex(0);
                        poolObject.SetActive(false);
                        UIManager.Instance.HideAllPopUps();
                        
                    }
                }
              else
                {
                    //sunAnim.Play("Sunset");
                    //sun.SetActive(false);
                }

            }
        }
        
    }
    private void LoadScriptableObjects()
    {
        // Load all ScriptableObjects of type WeatherType
        scriptableObjects = Resources.LoadAll<WeatherProfile>("");

        if (scriptableObjects == null || scriptableObjects.Length == 0)
        {
            Debug.LogError("No ScriptableObjects of type WeatherType found in the Resources folder.");
        }
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    public PoolObjectType AltocumulusType()
    {
        return PoolObjectType.Altocumulus;
    }

    public PoolObjectType AltostratusType()
    {
        return PoolObjectType.Altostratus;
    }

    public PoolObjectType Cirrostratus()
    {
        return PoolObjectType.Cirrostratus;
    }

    public PoolObjectType Cirrocomulus()
    {
        return PoolObjectType.Cirrucomulus;
    }

    public PoolObjectType Cirrus()
    {
        return PoolObjectType.Cirrus;
    }

    public PoolObjectType Cumulonimbus()
    {
        return PoolObjectType.Cumulonimbus;
    }

    public PoolObjectType Cumulus()
    {
        return PoolObjectType.Cumulus;
    }

    public PoolObjectType Stratcumulus()
    {
        return PoolObjectType.Stratcumulus;
    }

    public PoolObjectType Strus()
    {
        return PoolObjectType.Strutus;
    }


    public void ChangeWeatherFromKeyboard()
    {
        if (Input.GetKeyDown("1"))
        {
            LoadScriptableObjectAtIndex(0);
        }
        else if (Input.GetKeyDown("2"))
        {
            LoadScriptableObjectAtIndex(1);
        }
        else if (Input.GetKeyDown("3"))
        {
            LoadScriptableObjectAtIndex(2);
        }
        else if (Input.GetKeyDown("4"))
        {
            LoadScriptableObjectAtIndex(3);
        }
        else if (Input.GetKeyDown("5"))
        {
            LoadScriptableObjectAtIndex(4);
        }
        else if (Input.GetKeyDown("6"))
        {
            LoadScriptableObjectAtIndex(5);
        }
        else if (Input.GetKeyDown("7"))
        {
            LoadScriptableObjectAtIndex(6);
        }
        else if (Input.GetKeyDown("8"))
        {
            LoadScriptableObjectAtIndex(7);
        }
        else if (Input.GetKeyDown("9"))
        {
            LoadScriptableObjectAtIndex(8);
        }


    }

    IEnumerator WaitCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
    
    }






}
