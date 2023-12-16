using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

namespace FrameWork
{
    public interface IEventFunc
    {

    }

    public class EventFunc<T> : IEventFunc
    {
        public Func<T, UniTask> func;

        public EventFunc()
        {
            func = null;
        }

        public EventFunc(Func<T, UniTask> f)
        {
            func += f;
        }
    }

    public class EventFunc : IEventFunc
    {
        public Func<UniTask> func;

        public EventFunc()
        {
            func = null;
        }
        
        public EventFunc(Func<UniTask> f)
        {
            func += f;
        }
    }

    public class EventManager : SingletonManager<EventManager>
    {
        Dictionary<string, IEventFunc> _eventAsyncDic;

        public EventManager()
        {
            _eventAsyncDic = new Dictionary<string, IEventFunc>();
        }

        // 无参数版本的异步事件管理
#region 

        public void AddAsyncEventListener(string eventName, Func<UniTask> func)
        {
            if (_eventAsyncDic.ContainsKey(eventName))
            {
                (_eventAsyncDic[eventName] as EventFunc).func += func;
            }
            else
            {
                _eventAsyncDic.Add(eventName, new EventFunc(func));
            }
        }

        public void AddAsyncEventListener(string eventName, Func<UniTask> func, ref Action onDestroy)
        {
            AddAsyncEventListener(eventName, func);

            onDestroy += () => RemoveAsyncEventListener(eventName, func);
        }

        public void RemoveAsyncEventListener(string eventName, Func<UniTask> func)
        {
            if (_eventAsyncDic.ContainsKey(eventName))
            {
                (_eventAsyncDic[eventName] as EventFunc).func -= func;
            }
        }

        public async UniTask InvokeEventAsync(string eventName)
        {
            if (_eventAsyncDic.ContainsKey(eventName))
            {
                await UniTask.WhenAll((_eventAsyncDic[eventName] as EventFunc).func?.GetInvocationList().Select(x => (x as Func<UniTask>).Invoke()));
            }
        }
        
#endregion
    
        // 有参数版本的异步版本的事件管理器
#region

        public void AddAsyncEventListener<T>(string eventName, Func<T, UniTask> func)
        {
            if (!_eventAsyncDic.ContainsKey(eventName))
            {
                _eventAsyncDic.Add(eventName, new EventFunc<T>());
            }

            (_eventAsyncDic[eventName] as EventFunc<T>).func += func;
        }

        public void AddAsyncEventListener<T>(string eventName, Func<T, UniTask> func, ref Action onDestroy)
        {
            AddAsyncEventListener(eventName, func);

            onDestroy += () => RemoveAsyncEventListener(eventName, func);
        }

        public void RemoveAsyncEventListener<T>(string eventName, Func<T, UniTask> func)
        {
            if (_eventAsyncDic.ContainsKey(eventName))
            {
               (_eventAsyncDic[eventName] as EventFunc<T>).func -= func;
            }
        }

        public async UniTask InvokeEventAsync<T>(string eventName, T param)
        {
            if (_eventAsyncDic.ContainsKey(eventName))
            {
                await UniTask.WhenAll((_eventAsyncDic[eventName] as EventFunc<T>).func.GetInvocationList().Select( x => (x as Func<T, UniTask>).Invoke(param)));
            }
        }

        public void ClearAsyncEvent(string eventName)
        {
            _eventAsyncDic.Remove(eventName);
        }

        public void ClearAsyncEvents()
        {
            _eventAsyncDic.Clear();
        }

#endregion
    }
}