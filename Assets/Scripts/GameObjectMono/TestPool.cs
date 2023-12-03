using Cysharp.Threading.Tasks;
using FrameWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    public GameObject template1;
    public GameObject template2;

    private void Update()
    {
        TestPool();
    }

    private void Awake()
    {
        TestEvent();
        TestInput();    
        // TestPool();
    }

    public void TestInput()
    {
        InputManager.Instance.SwitchInput(true);
        EventManager.Instance.AddAsyncEventListener<float>("Horizontal", HandleInput);
        EventManager.Instance.AddAsyncEventListener<float>("Vertical", HandleInput);
        EventManager.Instance.AddAsyncEventListener<float>("Fire1", HandleInput);
    }

    public async UniTask HandleInput(float value)
    {
        Debug.Log((float)value);
    }
    
    /// <summary>
    /// 测试场景管理器模块,但是加载速度太快了看不出变化,NND
    /// </summary>
    public async void TestScene()
    {
        Debug.Log("BeforeLoadingScene");
        await GameSceneManager.Instance.LoadSceneAsync("TestNewScene");
        Debug.Log("AfterLoadingScene");
    }

    /// <summary>
    /// 测试公共Mono模块
    /// </summary>
    public void TestPublicMono()
    {
        NoMono n1 = new NoMono() { Message = "I'm OK" };
        NoMono n2 = new NoMono() { Message = "NO GOOD" };
        PublicMonoManager.Instance.ClearUpdateListener();
        PublicMonoManager.Instance.AddUpdateListener(n1.FakeUpdate);
        PublicMonoManager.Instance.ClearUpdateListener();
        PublicMonoManager.Instance.AddUpdateListener(n2.FakeUpdate);
    }

    /// <summary>
    /// 测试对象池模块
    /// </summary>
    public void TestPool()
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

    public void TestEvent()
    {
        EventManager.Instance.AddAsyncEventListener<PoolObject>("ObjectDestroy", ObjectDestroyHandler);
    }

    protected async UniTask ObjectDestroyHandler(PoolObject sender)
    {
        Debug.Log("Handler " + sender.ObjectName);
    }
}
