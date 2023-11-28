using FrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour, IPoolObject
{
    public string ObjectName = "UnKnown";
    public void OnPoolGet()
    {
        transform.localScale = Vector3.one * Random.Range(1f, 3f);
        ObjectName = Random.Range(1, 999).ToString();
        Invoke("ReturnSelf", 2f);
    }

    public void OnPoolReturn()
    {
        transform.localScale = Vector3.one;
        // �����¼�������
        EventManager.Instance.InvokeEvent("ObjectDestroy", this);

    }

    public void ReturnSelf()
    {
        PoolManager.Instance.ReturnObject(this.gameObject);
    }
}   
