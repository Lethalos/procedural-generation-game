using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public enum PoolObjectType
{
    Altocumulus,
    Altostratus,
    Cirrostratus,
    Cirrucomulus,
    Cirrus,
    Cumulonimbus,
    Cumulus,
    Stratcumulus,
    Strutus,
    None // Represents the null case
}

[Serializable]
public class PoolInfo
{
    [Space]
    public PoolObjectType type;
    public int amount = 0;
    public GameObject prefab;
    public GameObject container;
    public string tag;
    public Transform scale_;

    [HideInInspector]
    public List<GameObject> pool = new List<GameObject>();
    public Vector3 initialScale;

    public void GetInitialScale()
    {
        initialScale = prefab.transform.localScale;
    }
}

public class PoolManager : SingletonBehaviour<PoolManager>
{
    [SerializeField]
    public List<PoolInfo> listOfPool;
    public List<Transform> scaleList;

    private bool isScaleInreased=false;
    public event Action ScaleInreaseEvent;
    public event Action ScaleDecreaseEvent;

    private void Start()
    {
       
        StartCoroutine(WaitCoroutine(1f));
        


    }
    public void ActivatePoolObjects()
    {   
        for (int i = 0; i < listOfPool.Count; i++)
        {
            FillPool(listOfPool[i]);
            
            // Set the object in index 0 as active and assign the desired transform based on its type
            if (listOfPool[i].pool.Count > 0)
            {
                GameObject firstObject = listOfPool[i].pool[0];
                
                firstObject.transform.position = GetPrefabPosition(listOfPool[i].type);
                
            }
        }

        //StartCoroutine(IncreaseScalePrefabs(1.5f));
        IncreaseScale();
    }

    void FillPool(PoolInfo info)
    {
        for (int i = 0; i < info.amount; i++)
        {
            GameObject objInstance = Instantiate(info.prefab, info.container.transform);
            objInstance.gameObject.SetActive(false);
            objInstance.transform.position = GetPrefabPosition(info.type);
            info.pool.Add(objInstance);
            info.GetInitialScale();
        }
    }




    public GameObject GetPoolObject(PoolObjectType type)
    {
        PoolInfo selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        GameObject obInstance = null;
        if (pool.Count > 0)
        {
            obInstance = pool[pool.Count - 1];
            pool.Remove(obInstance);
        }
        else
        {
            obInstance = Instantiate(selected.prefab, selected.container.transform);
        }

        return obInstance;
    }

    public void PoolObject(GameObject ob, PoolObjectType type)
    {
        ob.SetActive(false);
        ob.transform.position = GetPrefabPosition(type);

        PoolInfo selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;
        if (!pool.Contains(ob))
        {
            pool.Add(ob);
        }
    }

    private PoolInfo GetPoolByType(PoolObjectType type)
    {
        for (int i = 0; i < listOfPool.Count; i++)
        {
            if (type == listOfPool[i].type)
                return listOfPool[i];
        }

        return null;
    }

    private Vector3 GetPrefabPosition(PoolObjectType type)
    {
        PoolInfo selected = GetPoolByType(type);
        GameObject prefab = selected.prefab;
        return prefab.transform.position;
    }

    private List<Transform> GetAllPrefabScales()
    {
        List<Transform> prefabScales = new List<Transform>();

        foreach (PoolInfo poolInfo in listOfPool)
        {
            prefabScales.Add(poolInfo.scale_);
        }

        return prefabScales;
    }
    private List<GameObject> GetAllPoolObjects()
    {
        List<GameObject> allObjects = new List<GameObject>();

        foreach (PoolInfo poolInfo in listOfPool)
        {
            allObjects.AddRange(poolInfo.pool);
        }

        return allObjects;
    }
    private PoolInfo GetPoolInfo(GameObject obj)
    {
        foreach (PoolInfo poolInfo in listOfPool)
        {
            if (poolInfo.pool.Contains(obj))
            {
                return poolInfo;
            }
        }
        return null;
    }

    public void IncreaseScale()
    {
        if(!isScaleInreased)
        {
            StopAllCoroutines();
            StartCoroutine(IncreaseScalePrefabs(1.5f));
            isScaleInreased = true;
            ScaleInreaseEvent?.Invoke();
        }
    }

    public void DecreaseScale()
    {
        if(isScaleInreased)
        {
            StopAllCoroutines();
            StartCoroutine(DecreaseScalePrefabs(1.5f));
            isScaleInreased = false;
            ScaleDecreaseEvent?.Invoke();
        }
    }

    private IEnumerator IncreaseScalePrefabs(float duration)
    {   
        Debug.Log("Scale increasing!");
        List<GameObject> allObjects = GetAllPoolObjects();

        List<Vector3> initialScales = new List<Vector3>();
        foreach (GameObject obj in allObjects)
        {
            PoolInfo poolInfo = GetPoolInfo(obj);
            if (poolInfo != null)
            {
                initialScales.Add(poolInfo.initialScale);
            }
            else
            {
                Debug.LogWarning("PoolInfo not found for GameObject: " + obj.name);
                initialScales.Add(Vector3.one);
            }

            obj.transform.localScale = Vector3.one;
            obj.SetActive(true);
        }

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            for (int i = 0; i < allObjects.Count; i++)
            {
                float t = Mathf.Clamp01(elapsedTime / duration);
                Vector3 targetScale = Vector3.Lerp(Vector3.one, initialScales[i], t);
                allObjects[i].transform.localScale = targetScale;
            }

            yield return null;
        }

        // Set scale to initial scales
        for (int i = 0; i < allObjects.Count; i++)
        {
            allObjects[i].transform.localScale = initialScales[i];
            allObjects[i].SetActive(true);
       

        }
    }



    private IEnumerator DecreaseScalePrefabs(float duration)
    {
        Debug.Log("Scale decreasing!");
        List<GameObject> allObjects = GetAllPoolObjects();

        List<Vector3> initialScales = new List<Vector3>();
        foreach (GameObject obj in allObjects)
        {
            PoolInfo poolInfo = GetPoolInfo(obj);
            if (poolInfo != null)
            {
                initialScales.Add(poolInfo.initialScale);
            }
            else
            {
                Debug.LogWarning("PoolInfo not found for GameObject: " + obj.name);
                initialScales.Add(Vector3.one);
            }
        }

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            for (int i = 0; i < allObjects.Count; i++)
            {
                float t = Mathf.Clamp01(elapsedTime / duration);
                Vector3 targetScale = Vector3.Lerp(initialScales[i], Vector3.one, t);
                allObjects[i].transform.localScale = targetScale;
            }

            yield return null;
        }

        // Deactivate objects and reset scale to initial scales
        foreach (GameObject obj in allObjects)
        {
            obj.SetActive(false);
            PoolInfo poolInfo = GetPoolInfo(obj);
            if (poolInfo != null)
            {
                obj.transform.localScale = poolInfo.initialScale;
            }
            else
            {
                Debug.LogWarning("PoolInfo not found for GameObject: " + obj.name);
                obj.transform.localScale = Vector3.zero;
            }
        }
    }



    public List<GameObject> GetAllObjectsOfType(PoolObjectType type)
    {
        PoolInfo selected = GetPoolByType(type);
        return selected.pool;
    }

    public PoolObjectType GetObjectTypeFromGameObject(GameObject gameObject)
    {
        foreach (PoolInfo poolInfo in listOfPool)
        {
            if (poolInfo.prefab.tag == gameObject.tag)
            {
                return poolInfo.type;
            }
        }

        return PoolObjectType.None;
    }

    public void DeactivatePoolObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void DeactivatePoolObjectsExcept(GameObject clickedObject)
    {
        PoolObjectType clickedObjectType = GetObjectTypeFromGameObject(clickedObject);

        foreach (PoolInfo poolInfo in listOfPool)
        {
            if (poolInfo.type != clickedObjectType)
            {
                foreach (GameObject obj in poolInfo.pool)
                {
                    DeactivatePoolObject(obj);
                }
            }
        }
    }


    public void DeactivateAllPoolObjects()
    {
        //foreach (PoolInfo poolInfo in listOfPool)
        //{
        //    foreach (GameObject obj in poolInfo.pool)
        //    {
        //        DeactivatePoolObject(obj);
        //    }
        //}


        //StartCoroutine(DecreaseScalePrefabs(1.5f));
        DecreaseScale();
        
    }
    public void ActivateSameTypeObjects(GameObject clickedObject)
    {
        PoolObjectType clickedObjectType = GetObjectTypeFromGameObject(clickedObject);

        foreach (PoolInfo poolInfo in listOfPool)
        {
            if (poolInfo.type == clickedObjectType)
            {
                int objectCount = poolInfo.pool.Count;
                for (int i = 0; i < objectCount; i++)
                {
                    GameObject obj = poolInfo.pool[i];
                    if (!obj.activeSelf)
                    {
                        obj.SetActive(true);

                        // Calculate the desired sampleCenter based on the index relative to the clicked object
                        int indexDifference = i - (objectCount / 2);
                        float offset = 0.6f; // Adjust the spacing between objects as needed
                        Vector3 position = GetPrefabPosition(clickedObjectType);

                        // Assign left, right, or center sampleCenter based on the index
                        if (i == 0)
                            position.x -= offset; // First object is on the left
                        else if (i == 1)
                            position.x += offset; // Second object is on the right
                        else if (i % 2 == 0)
                            position.x -= Mathf.CeilToInt(indexDifference / 2f) * offset; // Even-indexed objects are centered to the left
                        else
                            position.x += Mathf.CeilToInt((indexDifference - 1) / 2f) * offset; // Odd-indexed objects are centered to the right

                        StartCoroutine(ChangePositionWithDelay(obj, position, 1f));
                    }
                }
            }
        }
    }

    private IEnumerator ChangePositionWithDelay(GameObject obj, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.transform.DOMove(position, 0.5f);
    }

    //public void DeactivateSecondAndThirdObjects(GameObject clickedObject)
    //{
    //    PoolObjectType clickedObjectType = GetObjectTypeFromGameObject(clickedObject);
    //    if (clickedObjectType != PoolObjectType.None)
    //    {
    //        foreach (PoolInfo poolInfo in listOfPool)
    //        {
    //            if (poolInfo.type == clickedObjectType)
    //            {
    //                int objectCount = poolInfo.pool.Count;
    //                for (int i = 0; i < objectCount; i++)
    //                {
    //                    GameObject obj = poolInfo.pool[i];
    //                    if (!obj.activeSelf)
    //                    {
    //                        obj.SetActive(true);

    //                        // Calculate the desired sampleCenter based on the index relative to the clicked object
    //                        int indexDifference = i - (objectCount / 2);
    //                        float offset = 0.6f; // Adjust the spacing between objects as needed
    //                        Vector3 sampleCenter = GetPrefabPosition(clickedObjectType);

    //                        // Assign left, right, or center sampleCenter based on the index
    //                        if (i == 0)
    //                            sampleCenter.x -= offset; // First object is on the left
    //                        else if (i == 1)
    //                            sampleCenter.x += offset; // Second object is on the right
    //                        else if (i % 2 == 0)
    //                            sampleCenter.x -= Mathf.CeilToInt(indexDifference / 2f) * offset; // Even-indexed objects are centered to the left
    //                        else
    //                            sampleCenter.x += Mathf.CeilToInt((indexDifference - 1) / 2f) * offset; // Odd-indexed objects are centered to the right

    //                        StartCoroutine(ChangePositionWithDelay(obj, sampleCenter, 1f));
    //                    }
    //                }
    //            }
    //        }
    //    }
        
                
        
    //}

  
    public void ActivateFirstObjects()
    {
        foreach (PoolInfo poolInfo in listOfPool)
        {
            // Disable all objects in the pool
            foreach (GameObject obj in poolInfo.pool)
            {
                obj.SetActive(false);
            }

            // Enable the first object if the pool is not empty
            if (poolInfo.pool.Count > 0)
            {
                GameObject firstObject = poolInfo.pool[0];
                firstObject.SetActive(true);
                firstObject.transform.position = GetPrefabPosition(poolInfo.type);
            }
        }
        //StartCoroutine(IncreaseScalePrefabs(1.5f));
        IncreaseScale();
    }

    public IEnumerator WaitCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        ActivatePoolObjects();
    }
}
