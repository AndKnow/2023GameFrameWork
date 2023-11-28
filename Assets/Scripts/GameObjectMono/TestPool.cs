using FrameWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    public GameObject template1;
    public GameObject template2;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject go = PoolManager.Instance.GetObject(template1);
            go.transform.position = new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f), 0);
            Task.Delay(200).ContinueWith( x => PoolManager.Instance.ReturnObject(go));
        }
        if (Input.GetMouseButtonDown(1))
        {
            GameObject go = PoolManager.Instance.GetObject(template2);
            go.transform.position = new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f), 0);
            Task.Delay(200).ContinueWith( x => PoolManager.Instance.ReturnObject(go));
        }
    }

    private void Awake()
    {
        EventManager.Instance.AddEventListener("ObjectDestroy", ObjectDestroyHandler);
    }

    protected void ObjectDestroyHandler(object sender)
    {
        Debug.Log("Handler " + (sender as PoolObject).ObjectName);
    }
}
