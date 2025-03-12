using UnityEngine;

namespace YSH.Framework
{
    /// <summary>
    /// ���������
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
            //ˮƽ��
            EventMgr.Instance.EventDispatcher(InputEventName.Horizontal, Input.GetAxisRaw("Horizontal"));
            //��ֱ��
            EventMgr.Instance.EventDispatcher(InputEventName.Vertical, Input.GetAxisRaw("Vertical"));

            //����
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
