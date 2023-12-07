using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

namespace FrameWork
{
    public class UGUIPanelManager : SingletonManager<UGUIPanelManager>
    {
        protected Canvas _rootCanvas;
        public Canvas RootCanvas
        {
            get
            {
                if (_rootCanvas == null)
                {
                    _rootCanvas = new GameObject("GUIPanelRoot").AddComponent<Canvas>();
                    _rootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    _rootCanvas.gameObject.AddComponent<GraphicRaycaster>();
                }

                return _rootCanvas;
            }
        }
        Dictionary<string, UGUIPanelBaseController> _panelDic;

        public UGUIPanelManager()
        {
            _panelDic = new Dictionary<string, UGUIPanelBaseController>();
        }

        UGUIPanelBaseController GetPanel(string PanelName)
        {
            if (!_panelDic.ContainsKey(PanelName) || _panelDic[PanelName] == null)
            {
                _panelDic[PanelName] = PoolManager.Instance.GetObject("UGUIPanels/" + PanelName)?.GetComponentInChildren<UGUIPanelBaseController>();
            }

            return _panelDic[PanelName];
        }

        public void OpenPanel(string PanelName)
        {
            var panel = GetPanel(PanelName);
            if (panel != null)
            {
                panel.transform.SetParent(RootCanvas.transform);
                panel.transform.localPosition = Vector3.zero;
                panel.Show();
            }
        }

        public void ClosePanel(string PanelName)
        {
            if (!_panelDic.ContainsKey(PanelName))
                return;

            var panel = _panelDic[PanelName];
            if (panel != null)
            {
                panel.Hide();
            }
            else 
            {
                _panelDic.Remove(PanelName);
            }
        }

        public void TogglePanel(string PanelName)
        {
            var panel = GetPanel(PanelName);
            if (panel != null)
            {
                panel.transform.SetParent(RootCanvas.transform);
                panel.transform.localPosition = Vector3.zero;
                panel.ToggleState();
            }
        }
    }
}