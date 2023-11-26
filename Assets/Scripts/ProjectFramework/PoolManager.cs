using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolContainer
{
    GameObject _template;
    GameObject _container;
    List<GameObject> _poolList;
    public PoolContainer(GameObject poolRoot, GameObject template)
    {
        _template = template;
        _container = new GameObject(_template.name  + "Pool");
        _container.transform.parent = poolRoot.transform;
        _poolList = new List<GameObject>();
    }

    public GameObject GetObject()
    {
        GameObject go = null;
        if (_poolList.Count > 0)
        {
            go = _poolList[0];
            _poolList.RemoveAt(0);
            go.SetActive(true);
            go.transform.parent = null;
        }
        
        // 也是为了防止切换场景的时候移除了对象, 但是池中还保留了引用
        if (go == null)
        {
            go = GameObject.Instantiate(_template);
        }

        return go;
    }

    public void AddObject(GameObject go)
    {
        go.SetActive(false);
        go.transform.parent = _container.transform;
        _poolList.Add(go);
    }
}

public class PoolManager : SingletonManager<PoolManager>
{
    GameObject _poolRoot;
    Dictionary<GameObject, PoolContainer> _poolDic;

    public PoolManager()
    {
        _poolDic = new Dictionary<GameObject, PoolContainer>();
        _poolRoot = new GameObject("PoolRoot");
    }

    public GameObject GetObject(GameObject template)
    {
        return GetContainer(template).GetObject();
    }

    public void ReturnObject(GameObject obj)
    {
        GetContainer(obj).AddObject(obj);
    }

    protected PoolContainer GetContainer(GameObject obj)
    {
        // TODO,还有问题,通过GameObject创建的复用物体,还不到原来的池子里面去
        PoolContainer poolContainer = null;
        if (_poolDic.ContainsKey(obj))
        {
            poolContainer = _poolDic[obj];
        }
        else
        {
            poolContainer = new PoolContainer(_poolRoot, obj);
            _poolDic.Add(obj, poolContainer);
        }

        return poolContainer;
    }
}
