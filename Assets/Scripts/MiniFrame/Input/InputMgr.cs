
/// <summary>
/// 输入管理器
/// </summary>
public class InputMgr : Singleton<InputMgr>
{
    public bool IsEnableInput { get; set; }

    public InputMgr()
    {
        EventMgr.Instance.AddEventListener(MonoEventName.Update, Update);
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
            EventMgr.Instance.EventDispatcher("按下鼠标左键");
        }
        else if (UnityEngine.Input.GetMouseButton(0))
        {
            EventMgr.Instance.EventDispatcher("按住鼠标左键");
        }
        else if (UnityEngine.Input.GetMouseButtonUp(0))
        {
            EventMgr.Instance.EventDispatcher("抬起鼠标左键");
        }
    }
}
