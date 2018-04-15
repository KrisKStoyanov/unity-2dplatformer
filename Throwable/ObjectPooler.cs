using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    public static ObjectPooler current;

    public GameObject[] pooledObject;
    public int pooledAmountOne = 10;
    public int pooledAmountTwo = 2;
    public int pooledBackgroundAmountOne = 3;
    public int pooledBackgroundAmountTwo = 3;
    public int pooledBackgroundAmountThree = 3;
    public bool willGrow = true;

    public GameObject[] ParallaxSet;

    List<GameObject> pooledObjects;

    List<GameObject> pooledObjectsTwo;

    List<GameObject> pooledBackgroundOne;
    List<GameObject> pooledBackgroundTwo;
    List<GameObject> pooledBackgroundThree;

    private void Awake()
    {
        current = this;
    }


    // Use this for initialization
    void Start () {
        pooledObjects = new List<GameObject>();
        pooledObjectsTwo = new List<GameObject>();
        pooledBackgroundOne = new List<GameObject>();
        pooledBackgroundTwo = new List<GameObject>();
        pooledBackgroundThree = new List<GameObject>();
        for(int i = 0; i < pooledAmountOne; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject[0]);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
        for(int i = 0; i < pooledAmountTwo; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject[1]);
            obj.SetActive(false);
            pooledObjectsTwo.Add(obj);
        }
        for (int i = 0; i < pooledBackgroundAmountOne; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject[2]);
            obj.SetActive(false);
            pooledBackgroundOne.Add(obj);
            obj.transform.parent = ParallaxSet[0].transform;
        }
        for (int i = 0; i < pooledBackgroundAmountTwo; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject[3]);
            obj.SetActive(false);
            pooledBackgroundTwo.Add(obj);
            obj.transform.parent = ParallaxSet[1].transform;
        }
        for (int i = 0; i < pooledBackgroundAmountThree; i++)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject[4]);
            obj.SetActive(false);
            pooledBackgroundThree.Add(obj);
            obj.transform.parent = ParallaxSet[2].transform;
        }


    }

    public GameObject GetPooledObjectOne()
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject[0]);
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }

    public GameObject GetPooledObjectTwo()
    {
        for(int i = 0; i<pooledObjectsTwo.Count; i++)
        {
            if (!pooledObjectsTwo[i].activeInHierarchy)
            {
                return pooledObjectsTwo[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject[1]);
            pooledObjectsTwo.Add(obj);
            return obj;
        }
        return null;
    }

    public GameObject GetPooledBackgroundOne()
    {
        for (int i = 0; i < pooledBackgroundOne.Count; i++)
        {
            if (!pooledBackgroundOne[i].activeInHierarchy)
            {
                return pooledBackgroundOne[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject[2]);
            pooledBackgroundOne.Add(obj);
            return obj;
        }

        return null;
    }

    public GameObject GetPooledBackgroundTwo()
    {
        for (int i = 0; i < pooledBackgroundTwo.Count; i++)
        {
            if (!pooledBackgroundTwo[i].activeInHierarchy)
            {
                return pooledBackgroundTwo[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject[3]);
            pooledBackgroundTwo.Add(obj);
            return obj;
        }

        return null;
    }

    public GameObject GetPooledBackgroundThree()
    {
        for (int i = 0; i < pooledBackgroundThree.Count; i++)
        {
            if (!pooledBackgroundThree[i].activeInHierarchy)
            {
                return pooledBackgroundThree[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = (GameObject)Instantiate(pooledObject[4]);
            pooledBackgroundThree.Add(obj);
            return obj;
        }

        return null;
    }

}
