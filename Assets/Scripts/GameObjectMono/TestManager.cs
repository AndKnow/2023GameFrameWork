using Cysharp.Threading.Tasks;
using FrameWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class TestManager : MonoBehaviour
{
    public GameObject template1;
    public GameObject template2;

    private void Update()
    {
        // TestPool();
    }

    private void Awake()
    {
        // TestEvent();
        // TestInput();    
        // TestPool();
        // TestScene();
        ResourceManager.Instance.Initialize();
    }

    private void OnGUI()
    {
        // TestMusic();
        //TestUIManager();
        //TestUniTaskAddressables();
    }

    public void TestMusic()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "Play"))
        {
            MusicManager.Instance.PlayBackgroundMusic("BGM");
        }
        if (GUI.Button(new Rect(0, 100, 100, 100), "Stop"))
        {
            MusicManager.Instance.StopBackgroundMusic();
        }
        if (GUI.Button(new Rect(0, 200, 100, 100), "Pause"))
        {
            MusicManager.Instance.PauseBackgroundMusic();
        }
        if (GUI.Button(new Rect(0, 300, 100, 100), "ChangeVolume"))
        {
            MusicManager.Instance.ChangeBackgroundMusicVolume(0.5f);
        }
    }

    public void TestUIManager()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "OpenPanel"))
        {
            UGUIPanelManager.Instance.OpenPanel("TestUIPanel");
        }
        if (GUI.Button(new Rect(0, 100, 100, 100), "ClosePanel"))
        {
            UGUIPanelManager.Instance.ClosePanel("TestUIPanel");
        }
        if (GUI.Button(new Rect(0, 200, 100, 100), "TogglePanel"))
        {
            UGUIPanelManager.Instance.TogglePanel("TestUIPanel");
        }
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
    static bool isLoaded = false;
    public async void TestScene()
    {
        if (isLoaded)
            return;
        isLoaded = true;
        await UniTask.Delay(1000);
        Debug.Log("BeforeLoadingScene");
        await GameSceneManager.Instance.LoadSceneAsync("TestNewScene");
        Debug.Log("AfterLoadingScene");
        await UniTask.Delay(1000);
        await GameSceneManager.Instance.LoadSceneAsync("SampleScene");
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
            GameObject go = PoolManager.Instance.GetObject("Sphere");
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
