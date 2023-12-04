using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace FrameWork
{
    public abstract class SingletonManager<T> where T : new()
    {
        protected static T _instance;
        public static T Instance 
        {
            get 
            {
                if (_instance == null)
                    _instance = new T();

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        protected static bool _hasInitialized;
        public UniTask Initialize()
        {
            UniTask task1 = UniTask.CompletedTask, task2 = UniTask.CompletedTask;
            if (!_hasInitialized)
            {
                _hasInitialized = true;
                task1 = OnlyOnceInit();
            }
            task2 = MultipleInit();
            return UniTask.WhenAll(task1, task2);
        }

        protected virtual UniTask OnlyOnceInit()
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask MultipleInit()
        {
            return UniTask.CompletedTask;
        }
    }

}