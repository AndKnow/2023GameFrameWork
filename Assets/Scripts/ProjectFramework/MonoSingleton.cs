using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace FrameWork
{
    /// <summary>
    /// 1.继承MonoBehaviour的单例模式,比较优秀的实现方式
    /// 2.过场景的时候会被删除,不如普通的单例管理器
    /// </summary>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("MonoSingleton");
                        go.name = typeof(T).ToString();
                        _instance = go.AddComponent<T>();
                    }
                }   
                return _instance;
            }
        }
    }
}
