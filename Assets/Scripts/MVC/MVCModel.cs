using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace MVC
{
    public class MVCModel<T> where T : struct
    {
        protected T _data;
        public T Data => _data;
        Func<T,T> _updateHandler;

        // 数据的初始化、更新、存储
#region 

        public MVCModel<T> InitData(T data)
        {
            _data = data;
            return this;
        }

        public virtual void UpdateData(Func<T, T> callback = null)
        {
            if (callback != null)
            {
                _updateHandler = callback;
            }

           _data = _updateHandler?.Invoke(_data) ?? _data;
            _updateEvent?.Invoke(_data);
        }

        public virtual void SaveData()
        {
            
        }

#endregion

        // 数据更新的回调
#region

        protected event UnityAction<T> _updateEvent;
        public event UnityAction<T> UpdateEvent
        {
            add
            {
                _updateEvent += value;
            }
            remove
            {
                _updateEvent -= value;
            }
        }

#endregion
    }
}