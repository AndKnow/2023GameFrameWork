
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace FrameWork
{

    public class HandleInfo
    {
        protected uint _count = 0;
        protected AsyncOperationHandle _handle;

        public HandleInfo(AsyncOperationHandle handle)
        {
            _handle = handle;
            _count = 1;
        }

        public AsyncOperationHandle<T> GetHandle<T>()
        {
            _count++;
            return _handle.Convert<T>();
        }

        public bool Release<T>()
        {
            _count--;
            if (_count == 0)
            {
                Addressables.Release(_handle.Convert<T>());
                return true;
            }

            return false;
        }

        public void ForceRelease()
        {
            _count = 0;
            Addressables.Release(_handle);
        }
        
    }


    /// <summary>
    /// 基于Addressables的资源管理器
    /// </summary>
    public class ResourceManager : SingletonManager<ResourceManager> 
    {   

        // 初始化
#region 

        protected override UniTask OnlyOnceInit()
        {
            return UniTask.CompletedTask;
        }

        protected override UniTask MultipleInit()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            return UniTask.CompletedTask;
        }

#endregion

        string _ResourcePath = "";
        public T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(_ResourcePath + path) as T;
        }

        // 热更新版本的异步资源加载
#region 
        protected static Dictionary<string, HandleInfo> _loadedHandles = new Dictionary<string, HandleInfo>();
        protected static CancellationTokenSource _cts;

        public static async UniTask<T> LoadAsync<T>(string path) where T : Object
        {
            string key = typeof(T).Name + "_" + path;
            if (_loadedHandles.ContainsKey(key))
            {
                Debug.Log("LoadAllAsync from cache " + key);
                return _loadedHandles[key].GetHandle<T>().Result;
            }

            var handle = Addressables.LoadAssetAsync<T>(path);
            var result = await handle.ToUniTask(cancellationToken: _cts.Token).SuppressCancellationThrow();
            if (!result.IsCanceled)
            {
                //多个同时启动加载的话,可能会有多次插入
                if (!_loadedHandles.ContainsKey(key))
                {
                    _loadedHandles.Add(key, new HandleInfo(handle));
                    return handle.Result;
                }
                else
                {
                    Addressables.Release(handle);
                    return _loadedHandles[key].GetHandle<T>().Result;
                }
            }
            else
            {
                Debug.LogError("LoadAsync failure " + key);
                return null;
            }
        }

        /// <summary>
        /// 释放资源的时候也需要知道类型吗?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        public static void Release<T>(string path) where T : Object
        {
            string key = typeof(T).Name + "_" + path;
            if (_loadedHandles.ContainsKey(key))
            {
                if (_loadedHandles[key].Release<T>())
                {
                    _loadedHandles.Remove(key);
                }
            }
        }

        /// <summary>
        /// 根据泛型类型和名字+标签来动态加载资源
        /// 根据mergeMode不一样,需要缓存的资源也会不一样
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mergeMode"></param>
        /// <param name="releaseDependenciesOnFailure"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static async UniTask<IList<T>> LoadAllAsync<T>(Addressables.MergeMode mergeMode, bool releaseDependenciesOnFailure = true, params string[] keys) where T : Object
        {
            string key = typeof(T).Name + "_" + string.Join("_", keys) + "_" + mergeMode;
            if (_loadedHandles.ContainsKey(key))
            {
                Debug.Log("LoadAllAsync from cache " + key);
                return _loadedHandles[key].GetHandle<IList<T>>().Result;
            }

            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(new List<string>(keys), null, mergeMode, releaseDependenciesOnFailure);
            var result = await handle.ToUniTask(cancellationToken: _cts.Token).SuppressCancellationThrow();
            if (!result.IsCanceled)
            {
                //多个同时启动加载的话,可能会有多次插入
                if (!_loadedHandles.ContainsKey(key))
                {
                    _loadedHandles.Add(key, new HandleInfo(handle));
                    return handle.Result;
                }
                else
                {
                    Addressables.Release(handle);
                    return _loadedHandles[key].GetHandle<IList<T>>().Result;
                }
            }
            else
            {
                Debug.LogError("LoadAllAsync failure " + key);
                return null;
            }
        }

        public static void ReleaseAll<T>(Addressables.MergeMode mergeMode, params string[] keys) where T : Object
        {
            string key = typeof(T).Name + "_" + string.Join("_", keys) + "_" + mergeMode;
            if (_loadedHandles.ContainsKey(key))
            {
                if (_loadedHandles[key].Release<IList<T>>())
                {
                    _loadedHandles.Remove(key);
                }
            }
        }

        public static void ClearAll()
        {
            _loadedHandles.Foreach(x => x.Value.ForceRelease());
            _loadedHandles.Clear();
            AssetBundle.UnloadAllAssetBundles(true);
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

#endregion
    }
}