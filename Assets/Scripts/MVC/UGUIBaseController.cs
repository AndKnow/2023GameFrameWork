using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace MVC
{
    /// <summary>
    /// 如果一个面板绑定一个Controller的派生类,那么是否可以将其定义为类的单例?
    /// 但是如果对象消失的话,Controller也会消失,不如将数据设置为单例,Controller不设置为单例
    /// </summary>
    public abstract class UGUIBaseController<T> : MonoBehaviour where T : UGUIBaseModel<T>, new()
    {

        protected UGUIBaseView<T> _view;
        protected UGUIBaseView<T> _safeView
        {
            get
            {
                if (_view == null)
                {
                    _view = GetComponentInChildren<UGUIBaseView<T>>(true);
                    _view.Init();
                }
                
                Assert.IsNotNull(_view, $"{this.GetType()} 找不到view对象");
                return _view;
            }
        }

        protected T _data => UGUIBaseModel<T>.Instance as T;

        // 初始化
#region

        protected void Awake()
        {
            Init();
        }

        public virtual void Init()
        {
            InitModelCallback();
            InitViewCallback();
        }

#endregion

        // 监听输入和变化
#region 

        protected abstract void InitViewCallback();

        /// <summary>
        /// 根据情况监听和处理view上面的控件变化
        /// </summary>
        protected abstract void HandleControls();

#endregion

        // 刷新显示
#region 

        protected virtual void InitModelCallback()
        {
            _data.UpdateEvent += RefreshView;
        }

        /// <summary>
        /// 当数据发生变化的时候触发回调调用这个函数
        /// </summary>
        /// <param name="data"></param>
        protected virtual void RefreshView(T data)
        {
            _safeView.RefreshView(data);
        }

#endregion
    }
}