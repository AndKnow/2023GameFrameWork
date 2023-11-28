using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace FrameWork
{

    public class EventManager : SingletonManager<EventManager>
    {
        Dictionary<string, UnityAction<object>> _eventDic = new Dictionary<string, UnityAction<object>>();

        public void AddEventListener(string eventName, UnityAction<object> action)
        {
            if (_eventDic.ContainsKey(eventName))
            {
                _eventDic[eventName] += action;
            }
            else
            {
                _eventDic.Add(eventName, action);
            }
        }

        public void RemoveEventListener(string eventName, UnityAction<object> action)
        {
            if (_eventDic.ContainsKey(eventName))
            {
                _eventDic[eventName] -= action;
            }
        }

        public void InvokeEvent(string eventName, object param = null)
        {
            if (_eventDic.ContainsKey(eventName))
            {
                _eventDic[eventName]?.Invoke(param);
            }
        }

        public void ClearEvent(string eventName)
        {
            _eventDic.Remove(eventName);
        }

        public void ClearEvents()
        {
            _eventDic.Clear();
        }
    }
}