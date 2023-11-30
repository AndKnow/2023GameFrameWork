using Cysharp.Threading.Tasks;
using UnityEngine;

namespace FrameWork
{
    public class ResourceManager : SingletonManager<ResourceManager> 
    {   
        string _ResourcePath = "Assets/Resources/";
        public object Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public async UniTask<T> LoadAsync<T>(string path) where T : Object
        {
            return await Resources.LoadAsync<T>(path).ToUniTask() as T;
        }

        public async UniTask<T> InstantiateAsync<T>(GameObject obj) where T : Object
        {
            return await UniTask.Create(async () => 
            {
                return GameObject.Instantiate(obj);
            }) as T;
        }
    }
}