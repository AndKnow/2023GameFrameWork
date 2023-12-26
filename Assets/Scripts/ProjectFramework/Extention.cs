using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public static class Extension
{
    /// <summary>
    /// 用于Dictionary的扩展方法,清除key为null的键值对
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dic"></param>
    public static void ClearNullKeys<T>(this Dictionary<GameObject, T> dic) 
    {
        foreach (var pair in dic.ToArray())
        {
            if (pair.Key == null)
            {
                dic.Remove(pair.Key);
            }
        }
    }

    /// <summary>
    /// 获取单例Mono组件,先遍历查找再创建,可能有一定的性能开销
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetSingletonMonoComponent<T> () where T : MonoBehaviour
    {
        var component = GameObject.FindObjectOfType<T>(true);
        if (component == null)
        {
            component = new GameObject(typeof(T).ToString()).AddComponent<T>();
        }
        component.gameObject.SetActive(true);
        return component;
    }

    /// <summary>
    /// 获取单例Mono组件,先遍历查找再创建,可能有一定的性能开销
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetSingletonComponent<T> () where T : Component
    {
        var component = GameObject.FindObjectOfType<T>(true);
        if (component == null)
        {
            component = new GameObject(typeof(T).ToString()).AddComponent<T>();
        }
        component.gameObject.SetActive(true);
        return component;
    }

    public static void Foreach<T>(this IEnumerable<T> collections, Action<T> action)
    {
        foreach (var item in collections)
        {
            action(item);
        }
    }
}