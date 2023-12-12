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
            InitTestMVCModel();
        }

        protected void OnGUI()
        {
            TestMVCModel();
        }

        MVCModel<int> _data;
        public void InitTestMVCModel()
        {
            _data = new MVCModel<int>().InitData(500);
            _data.UpdateEvent += OnModelDataChanged;
        }

        public void TestMVCModel()
        {
            if (GUI.Button(new Rect(0, 0, 100, 100), "UpdateData"))
            {
                _data.UpdateData(x =>
                {
                    return x + 100;
                });
            }

        }

        public void OnModelDataChanged(int data)
        {
            Debug.Log("OnModelDataChanged: " + data);
        }
    }
}