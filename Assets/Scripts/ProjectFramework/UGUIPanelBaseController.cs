using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FrameWork
{
    public class UGUIPanelBaseController : MonoBehaviour
    {
        Dictionary<string, List<UIBehaviour>> _componentsDic = new Dictionary<string, List<UIBehaviour>>();

        // 初始化
#region 

        protected void Awake()
        {
            FindAllComponents();
            InitCallBack();
        }
        
        protected virtual void InitCallBack()
        {
            
        }

#endregion

        // 子对象UI查找
#region 

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

        protected void FindAllComponents<T>() where T : UIBehaviour
        {
            T[] components = GetComponentsInChildren<T>(true);
            foreach (var component in components)
            {
                string componentName = component.gameObject.name;
                if (!_componentsDic.ContainsKey(componentName))
                {
                    _componentsDic.Add(componentName, new List<UIBehaviour>());
                }
                
                _componentsDic[componentName].Add(component);
            }
        }

        public T GetUIComponent<T> (string componentName) where T : UIBehaviour
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
    
        // 面板的显示和隐藏
#region 

        public virtual void Show()
        {
            gameObject.SetActive(true);
        } 

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

#endregion

    }
}