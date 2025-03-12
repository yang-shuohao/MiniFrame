using UnityEngine;

namespace YSH.Framework
{
    /// <summary>
    /// 输入管理器
    /// </summary>
    public class InputMgr : Singleton<InputMgr>
    {
        public bool IsEnableInput { get; set; }

        public InputMgr()
        {
            MonoMgr.Instance.AddUpdateListener(Update);
        }

        private void Update()
        {
            if (IsEnableInput)
            {
                CheckInput();
            }
        }

        private void CheckInput()
        {
            //水平轴
            EventMgr.Instance.EventDispatcher(InputEventName.Horizontal, Input.GetAxisRaw("Horizontal"));
            //垂直轴
            EventMgr.Instance.EventDispatcher(InputEventName.Vertical, Input.GetAxisRaw("Vertical"));

            //按键
            CheckKeyCode(KeyCode.W);
            CheckKeyCode(KeyCode.S);
            CheckKeyCode(KeyCode.A);
            CheckKeyCode(KeyCode.D);

        }

        private void CheckKeyCode(KeyCode keyCode)
        {
            if (Input.GetKeyDown(keyCode))
            {
                EventMgr.Instance.EventDispatcher(InputEventName.KeyDown, keyCode);
            }

            if (Input.GetKeyUp(keyCode))
            {
                EventMgr.Instance.EventDispatcher(InputEventName.KeyUp, keyCode);
            }

            if (Input.GetKey(keyCode))
            {
                EventMgr.Instance.EventDispatcher(InputEventName.KeyHold, keyCode);
            }
        }
    }
}
