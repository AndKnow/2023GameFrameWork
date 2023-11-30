using UnityEngine;

namespace FrameWork
{
    public class InputManager : SingletonManager<InputManager>
    {

        public InputManager()
        {
            PublicMonoManager.Instance.AddUpdateListener(Update);
        }

        bool InputOpen = true;
        // 切换输入开关
        public void SwitchInput(bool open)
        {
            InputOpen = open;
        }

        protected void Update()
        {
            if (InputOpen)
            {
                InputAndInvoke("Horizontal");
                InputAndInvoke("Vertical");
                InputAndInvoke("Fire1");
            }
        }

        protected void InputAndInvoke(string axis)
        {
            float value = Input.GetAxis(axis);
            if (value != 0)
            {
                EventManager.Instance.InvokeEvent(axis, value);
            }
        }
    }
}