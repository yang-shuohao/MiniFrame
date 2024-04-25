using MiniFrame.Base;
using MiniFrame.Event;
using MiniFrame.Mono;

namespace MiniFrame.Input
{
    /// <summary>
    /// 输入管理器
    /// </summary>
    public class InputManager : Singleton<InputManager>
    {
        public bool IsEnableInput { get; set; }

        public InputManager()
        {
            MonoManager.Instance.AddUpdateListener(Update);
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
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                EventManager.Instance.EventDispatcher("按下鼠标左键");
            }
            else if (UnityEngine.Input.GetMouseButton(0))
            {
                EventManager.Instance.EventDispatcher("按住鼠标左键");
            }
            else if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                EventManager.Instance.EventDispatcher("抬起鼠标左键");
            }
        }
    }
}
