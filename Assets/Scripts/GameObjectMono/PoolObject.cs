using Cysharp.Threading.Tasks;
using FrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour, IPoolObject
{
    public string ObjectName = "UnKnown";
    public async void OnPoolGet()
    {
        transform.localScale = Vector3.one * Random.Range(1f, 3f);
        transform.localPosition = Vector3.one * Random.Range(1, 10);
        ObjectName = Random.Range(1, 999).ToString();

        this.GetComponent<MeshRenderer>().material = await ResourceManager.LoadAsync<Material>("ActiveMaterial");
        Invoke("ReturnSelf", 2f);
    }

    public void OnPoolReturn()
    {
        transform.localScale = Vector3.one;
        // 测试事件管理器
        EventManager.Instance.InvokeEventAsync<PoolObject>("ObjectDestroy", this).Forget();

    }

    public void ReturnSelf()
    {
        PoolManager.Instance.ReturnObject(this.gameObject);
    }
}   
