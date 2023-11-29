using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace FrameWork
{
    public class MonoController : MonoBehaviour
    {
        protected event UnityAction _updateEvent;
        public event UnityAction UpdateEvent
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

        protected void Update()
        {
            _updateEvent?.Invoke();
        }

        public void Clear()
        {
            _updateEvent = null;
        }
    }


    public class PublicMonoManager : SingletonManager<PublicMonoManager>
    {
        protected MonoController _monoController;
        protected MonoController MonoController
        {
            get 
            {
                if (_monoController == null)
                    _monoController = Extension.GetSingletonMonoComponent<MonoController>();
                return _monoController;
            }
        }
        
        public PublicMonoManager()
        {

        }

        public void AddUpdateListener(UnityAction action)
        {
            MonoController.UpdateEvent += action;
        }

        public void RemoveUpdateListener(UnityAction action)
        {
            MonoController.UpdateEvent -= action;
        }

        public void ClearUpdateListener()
        {
            MonoController.Clear();
        }
    }
}