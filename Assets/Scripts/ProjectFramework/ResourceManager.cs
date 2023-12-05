using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FrameWork
{
    public class ResourceManager : SingletonManager<ResourceManager> 
    {   
        string _ResourcePath = "";
        public T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(_ResourcePath + path) as T;
        }

        public async UniTask<T> LoadAsync<T>(string path) where T : Object
        {
            return await Resources.LoadAsync<T>(_ResourcePath + path).ToUniTask() as T;
        }

        /// <summary>
        /// TODO,还没有实现异步的创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async UniTask<T> InstantiateAsync<T>(GameObject obj) where T : Object
        {
            return await UniTask.Create(async () => 
            {
                await UniTask.DelayFrame(1);
                return GameObject.Instantiate(obj);
            }) as T;
        }
    }
}