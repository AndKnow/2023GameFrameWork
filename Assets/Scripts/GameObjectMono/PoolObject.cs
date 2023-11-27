using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour, IPoolObject
{
    public void OnPoolGet()
    {
        transform.localScale = Vector3.one * Random.Range(1f, 3f);
        Invoke("ReturnSelf", 2f);
    }

    public void OnPoolReturn()
    {
        transform.localScale = Vector3.one;
    }

    public void ReturnSelf()
    {
        PoolManager.Instance.ReturnObject(this.gameObject);
    }
}   
