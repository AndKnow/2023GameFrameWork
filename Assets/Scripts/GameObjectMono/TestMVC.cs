using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace MVC
{
    public class TestMVC : MonoBehaviour
    {
        private void Awake()
        {
            
        }

        protected void OnGUI()
        {
            //TestMVCPanel();
        }

        private static void TestMVCPanel()
        {
            if (GUI.Button(new Rect(0, 0, 100, 100), "Call MVC Panel"))
            {
                UGUIPanelManager.Instance.OpenPanel("MVCPanel");
            }
            if (GUI.Button(new Rect(0, 100, 100, 100), "close MVC Panel2"))
            {
                UGUIPanelManager.Instance.ClosePanel("MVCPanel");
            }
        }

        public void OnConnectedToServer()
        {
            
        }

        public void OnModelDataChanged(int data)
        {
            Debug.Log("OnModelDataChanged: " + data);
        }
    }
}