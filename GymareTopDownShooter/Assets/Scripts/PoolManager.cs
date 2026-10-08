using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Transform prefab;
    [SerializeField] int amount;
    List<Transform> pool = new List<Transform>();

    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Transform newObj = Instantiate(prefab, transform);
            newObj.gameObject.SetActive(false);
            pool.Add(newObj);
        }
    }

    public Transform Enqeue()
    {
        Transform given = pool[pool.Count-1] != null ? pool[pool.Count - 1] : Instantiate(prefab, transform);
        given.gameObject.SetActive(false);
        pool.RemoveAt(0);
        return given;
    }

    public void Reqeue(Transform obj)
    {
        pool.Add(obj);
        obj.gameObject.SetActive(false);
    }
}