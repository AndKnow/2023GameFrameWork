using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

namespace FrameWork
{

    public class EventManager : SingletonManager<EventManager>
    {
        Dictionary<string, UnityAction<object>> _eventDic = new Dictionary<string, UnityAction<object>>();
        Dictionary<string, List<Func<object, UniTask>>> _eventAsyncDic = new Dictionary<string, List<Func<object, UniTask>>>();

        // 普通版本的事件管理器
#region 

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
        
#endregion
    
        // 异步版本的事件管理器
#region

        public void AddEventListenerAsync(string eventName, Func<object, UniTask> action)
        {
            if (!_eventAsyncDic.ContainsKey(eventName))
            {
                _eventAsyncDic.Add(eventName, new List<Func<object, UniTask>>());
            }

            _eventAsyncDic[eventName].Add(action);
        }

        public void RemoveEventListenerAsync(string eventName, Func<object, UniTask> action)
        {
            if (_eventAsyncDic.ContainsKey(eventName))
            {
                _eventAsyncDic[eventName].Remove(action);
            }
        }

        public async UniTask InvokeEventAsync(string eventName, object param = null)
        {
            if (_eventAsyncDic.ContainsKey(eventName))
            {
                await UniTask.WhenAll(_eventAsyncDic[eventName].Select(x => x.Invoke(param)));
            }
        }

        public void ClearEventAsync(string eventName)
        {
            _eventAsyncDic.Remove(eventName);
        }

        public void ClearEventsAsync()
        {
            _eventAsyncDic.Clear();
        }

#endregion
    }
}