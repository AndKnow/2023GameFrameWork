using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace MVC
{   
    /// <summary>
    /// 数据全部设置为静态的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UGUIBaseModel<T> where T : UGUIBaseModel<T>, new()
    {


        // 数据的初始化、更新、存储
#region     
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                    _instance.InitData();
                }

                return _instance;
            }
        }

        public abstract T InitData();

        public virtual void UpdateData()
        {
            ConcreteUpdate();
            _updateEvent?.Invoke(this as T);
        }

        public virtual void UpdateDataWithoutNotify()
        {
            ConcreteUpdate();
        }

        public abstract void ConcreteUpdate();

        public abstract void SaveData();

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