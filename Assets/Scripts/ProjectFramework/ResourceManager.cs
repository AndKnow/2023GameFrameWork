
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FrameWork
{
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
        protected static Dictionary<string, IEnumerator> _loadedHandles = new Dictionary<string, IEnumerator>();
        protected static CancellationTokenSource _cts;

        public static async UniTask<T> LoadAsync<T>(string path) where T : Object
        {
            string key = typeof(T).Name + "_" + path;
            if (_loadedHandles.ContainsKey(key))
            {
                Debug.Log("LoadAllAsync from cache " + key);
                return ((AsyncOperationHandle<T>)_loadedHandles[key]).Result;
            }

            var handler = Addressables.LoadAssetAsync<T>(path);
            var result = await handler.ToUniTask(cancellationToken: _cts.Token).SuppressCancellationThrow();
            if (!result.IsCanceled)
            {
                _loadedHandles.Add(key, handler);
                return handler.Result;
            }
            else
            {
                Debug.LogError("LoadAsync failure " + key);
                return null;
            }
        }

        public static void Release<T>(string path) where T : Object
        {
            string key = typeof(T).Name + path;
            if (_loadedHandles.ContainsKey(key))
            {
                Addressables.Release((AsyncOperationHandle<T>)_loadedHandles[key]);
                _loadedHandles.Remove(key);
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
                return ((AsyncOperationHandle<IList<T>>)_loadedHandles[key]).Result;
            }

            AsyncOperationHandle<IList<T>> handler = Addressables.LoadAssetsAsync<T>(new List<string>(keys), null, mergeMode, releaseDependenciesOnFailure);
            var result = await handler.ToUniTask(cancellationToken: _cts.Token).SuppressCancellationThrow();
            if (!result.IsCanceled)
            {
                _loadedHandles.Add(key, handler);
                return (handler.Result).ToList();
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
                Addressables.Release((AsyncOperationHandle<List<T>>)_loadedHandles[key]);
                _loadedHandles.Remove(key);
            }
        }

#endregion
    }
}