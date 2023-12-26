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

        GetComponent<MeshRenderer>().material = await ResourceManager.LoadAsync<Material>("ActiveMaterial");
        Invoke("ReturnSelf", 2f);
    }

    public void OnPoolReturn()
    {
        // ���Զ���س�ʼ��
        transform.localScale = Vector3.one;
        //������Դ�����������ͷ�
        ResourceManager.Release<Material>("ActiveMaterial");
        // �����¼�������
        EventManager.Instance.InvokeEventAsync<PoolObject>(EventTiming.GameTiming.OnObjectDestroy, this).Forget();

    }

    public void ReturnSelf()
    {
        PoolManager.Instance.ReturnObject(this.gameObject);
    }
}   
