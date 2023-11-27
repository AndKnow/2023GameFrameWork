using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Assertions;

public interface IPoolObject
{
    void OnPoolGet();
    void OnPoolReturn();
}

public class PoolContainer
{
    GameObject _template;
    GameObject _containerObject;
    GameObject _safeContainerObject
    {
        get
        {
            if (_containerObject == null)
            {
                _containerObject = new GameObject(_template.name  + "Pool");
                _containerObject.transform.parent = PoolManager.Instance.SafePoolRoot.transform;
            }
            return _containerObject;
        }
    }
    List<GameObject> _poolList;
    public PoolContainer(GameObject poolRoot, GameObject template)
    {
        _template = template;
        _poolList = new List<GameObject>();
    }

    public GameObject GetObject()
    {
        GameObject go = null;
        if (_poolList.Count > 0)
        {
            go = _poolList[0];
            _poolList.RemoveAt(0);
        }
        
        // 也是为了防止切换场景的时候移除了对象, 但是池中还保留了引用
        if (go == null)
        {
            go = GameObject.Instantiate(_template);
        }

        go.SetActive(true);
        go.transform.parent = null;

        return go;
    }

    public void AddObject(GameObject go)
    {
        go.SetActive(false);
        go.transform.parent = _safeContainerObject.transform;
        _poolList.Add(go);
    }
}

public class PoolManager : SingletonManager<PoolManager>
{
    GameObject _poolRoot;
    public GameObject SafePoolRoot
    {
        get
        {
            if (_poolRoot == null)
            {
                _poolRoot = new GameObject("PoolRoot");
            }
            return _poolRoot;
        }
    }
    Dictionary<GameObject, PoolContainer> _poolDic;
    Dictionary<GameObject, PoolContainer> _returnDic;
    Dictionary<GameObject, IPoolObject[]> _poolObjectDic;

    public PoolManager()
    {
        _poolDic = new Dictionary<GameObject, PoolContainer>();
        _returnDic = new Dictionary<GameObject, PoolContainer>();
        _poolObjectDic = new Dictionary<GameObject, IPoolObject[]>();
        _poolRoot = new GameObject("PoolRoot");
    }

    public GameObject GetObject(GameObject template)
    {
        var container = GetContainer(template);
        var go = container.GetObject();
        Assert.IsNotNull(go, "GameObject Pool Exception : " + template.name);

        if (!_returnDic.ContainsKey(go))
        {
            _returnDic.Add(go, container); 
        }
        else 
        {
            _returnDic[go] = container;
        }

        if (!_poolObjectDic.ContainsKey(go))
        {
            _poolObjectDic.Add(go, go.GetComponentsInChildren<IPoolObject>());
        }
        foreach (var poolObject in _poolObjectDic[go])
        {
            poolObject.OnPoolGet();
        }

        return go;
    }

    public void ReturnObject(GameObject obj)
    {
        foreach (var poolObject in _poolObjectDic[obj])
        {
            poolObject.OnPoolReturn();
        }

        GetContainer(obj, true).AddObject(obj);
    }
 
    protected PoolContainer GetContainer(GameObject obj, bool isReturn = false)
    {
        Dictionary<GameObject, PoolContainer> tmpDic = isReturn ? _returnDic : _poolDic;
        PoolContainer poolContainer = null;
        if (tmpDic.ContainsKey(obj))
        {
            poolContainer = tmpDic[obj];
        }
        else
        {
            poolContainer = new PoolContainer(_poolRoot, obj);
            tmpDic.Add(obj, poolContainer);
        }

        return poolContainer;
    }
}
