using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        Invoke("Push", 2f);
        
    }

    public void Push()
    {
        PoolManager.Instance.ReturnObject(this.gameObject);
    }
}   
