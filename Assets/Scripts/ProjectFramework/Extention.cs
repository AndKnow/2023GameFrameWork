using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public static class Extension
{
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
}