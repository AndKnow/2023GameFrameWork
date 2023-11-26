using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    public GameObject template;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject go = PoolManager.Instance.GetObject(template);
            go.transform.position = new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f), 0);
            Task.Delay(200).ContinueWith( x => PoolManager.Instance.ReturnObject(go));
        }
    }   
}
