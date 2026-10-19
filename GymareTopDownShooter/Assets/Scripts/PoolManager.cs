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
        if (pool.Count > 0)
        {
            Transform given = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
            return given;
        }

        return Instantiate(prefab, transform);
    }

    public void Reqeue(Transform obj)
    {
        pool.Add(obj);
    }
}