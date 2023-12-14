using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace MVC
{
    public abstract class UGUIBaseView<T> : MonoBehaviour where T : UGUIBaseModel<T>, new()
    {

        // 初始化
#region 

        public virtual void Init()
        {
            FindAllComponents();
        }

#endregion

        // 子对象绑定和获取
#region 

        Dictionary<string, List<Component>> _componentsDic = new Dictionary<string, List<Component>>();
        protected void FindAllComponents()
        {
            FindAllComponents<Button>();
            FindAllComponents<Image>();
            FindAllComponents<Text>();
            FindAllComponents<Slider>();
            FindAllComponents<Scrollbar>();
            FindAllComponents<Toggle>();
            FindAllComponents<InputField>();
            FindAllComponents<ScrollRect>();
            FindAllComponents<Dropdown>();
        }

        protected void FindAllComponents<T>() where T : Component
        {
            T[] components = GetComponentsInChildren<T>(true);
            foreach (var component in components)
            {
                string componentName = component.gameObject.name;
                if (!_componentsDic.ContainsKey(componentName))
                {
                    _componentsDic.Add(componentName, new List<Component>());
                }
                
                _componentsDic[componentName].Add(component);
            }
        }

        public T GetUIComponent<T> (string componentName) where T : Component
        {
            if (_componentsDic.ContainsKey(componentName))
            {
                foreach(var com in _componentsDic[componentName])
                {
                    if (com is T)
                    {
                        return com as T;
                    }
                }
            }

            return null;
        }

#endregion

        // 自身控制和显示的刷新
#region 

        public virtual void Show()
        {
            gameObject.SetActive(true);
            OnShow();
        } 

        public virtual void OnShow() {}

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void ToggleState()
        {
            if (gameObject.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
        
        public abstract void RefreshView(T data);

#endregion

    }
}