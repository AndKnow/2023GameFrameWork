using FrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TestAddressables : MonoBehaviour
{
    private void Awake()
    {
        ResourceManager.Instance.Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        TestUniTaskAddressables();
    }

    public async void TestUniTaskAddressables()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "LoadSingleByName"))
        {
            var result = await ResourceManager.LoadAsync<GameObject>("Sphere");
            PoolManager.Instance.GetObject(result).transform.SetParent(transform, false);
            Debug.Log("LoadSingleByName " + result.gameObject.name);
        }

        if (GUI.Button(new Rect(0, 100, 100, 100), "LoadSingleByLabel"))
        {
            var result = await ResourceManager.LoadAsync<GameObject>("Object");
            PoolManager.Instance.GetObject(result).transform.SetParent(transform, false);
            Debug.Log("LoadSingleByLabel " + result.gameObject.name);
        }

        if (GUI.Button(new Rect(0, 200, 100, 100), "LoadMultipleIntersection"))
        {
            var result = await ResourceManager.LoadAllAsync<GameObject>(mergeMode: Addressables.MergeMode.Intersection, releaseDependenciesOnFailure: true, "Cube", "Object");
            foreach (var item in result)
            {
                PoolManager.Instance.GetObject(item).transform.SetParent(transform, false);
                Debug.Log("LoadMultipleIntersection " + item.gameObject.name);
            }
        }

        if (GUI.Button(new Rect(0, 300, 100, 100), "LoadMultipleUnion"))
        {
            var result = await ResourceManager.LoadAllAsync<GameObject>(mergeMode: Addressables.MergeMode.Union, releaseDependenciesOnFailure: true, "Sphere", "Object");
            foreach (var item in result)
            {
                PoolManager.Instance.GetObject(item).transform.SetParent(transform, false);
                Debug.Log("LoadMultipleUnion " + item.gameObject.name);
            }
        }

        if (GUI.Button(new Rect(0, 400, 100, 100), "ClearAddressables"))
        {
            ResourceManager.ClearAll();
        }
    }
}
