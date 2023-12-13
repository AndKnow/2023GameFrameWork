using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MVC
{
    /// <summary>
    /// 如果一个面板绑定一个Controller的派生类,那么是否可以将其定义为类的单例?
    /// 但是如果对象消失的话,Controller也会消失,不如将数据设置为单例,Controller不设置为单例
    /// </summary>
    public abstract class UGUIBaseController : MonoBehaviour
    {

        protected UGUIBaseView _view;
        protected UGUIBaseView _safeView
        {
            get
            {
                if (_view == null)
                {
                    _view = GetComponentInChildren<UGUIBaseView>();
                    _view.Init();
                }

                return _view;
            }
        }

        protected abstract UGUIBaseModel _model { get; }

        // 监听输入
#region 

        public abstract void InitViewCallback();

#endregion

        // 更新数据
#region 

        /// <summary>
        /// 根据情况监听和处理view上面的控件变化
        /// </summary>
        public virtual void HandleControls()
        {

        }

#endregion

        // 刷新显示
#region 

        public virtual void InitModelCallback()
        {
            _model.UpdateEvent += RefreshView;
        }

        public virtual void RefreshView(UGUIBaseModel model)
        {
            _safeView.RefreshView(model);
        }

#endregion
    }
}