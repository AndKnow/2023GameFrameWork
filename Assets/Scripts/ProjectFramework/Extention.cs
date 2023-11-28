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
}