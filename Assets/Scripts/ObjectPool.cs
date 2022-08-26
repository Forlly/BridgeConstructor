using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public static ObjectPool Instance;

    [SerializeField] private List<GameObject> poolObjects = new List<GameObject>();
    [SerializeField] private int amountPool = 30;
    [SerializeField] private GameObject platform;

    private bool isFull = false;

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < amountPool; i++)
        {
            GameObject tmpObj = Instantiate(platform);
            tmpObj.SetActive(false);
            poolObjects.Add(tmpObj);
        }
    }


    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountPool; i++)
        {
            if (!poolObjects[i].activeInHierarchy)
            {
                poolObjects[i].SetActive(true);
                return poolObjects[i];
            }

            isFull = true;
        }

        if (isFull)
        {
            return CreateNewObject();
        }
        return null;
    }
    
    public  void TurnOfObject( GameObject _platform)
    {
        for (int i = 0; i < amountPool; i++)
        {
            if (_platform == poolObjects[i])
            {
                poolObjects[i].SetActive(false);
            }
            
        }
    }

    private GameObject CreateNewObject()
    {
        GameObject tmpObj = Instantiate(platform);
        tmpObj.SetActive(true);
        poolObjects.Add(tmpObj);
        return tmpObj;
    }
    
}
