using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace MVC
{
    public abstract class UGUIBaseModel
    {


        // 数据的初始化、更新、存储
#region 

        public abstract UGUIBaseModel InitData<T>(T data);

        public virtual void UpdateData()
        {
            ConcreteUpdate();
            _updateEvent?.Invoke(this);
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

        protected event UnityAction<UGUIBaseModel> _updateEvent;
        public event UnityAction<UGUIBaseModel> UpdateEvent
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