
/// <summary>
/// ���������
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
            EventMgr.Instance.EventDispatcher("����������");
        }
        else if (UnityEngine.Input.GetMouseButton(0))
        {
            EventMgr.Instance.EventDispatcher("��ס������");
        }
        else if (UnityEngine.Input.GetMouseButtonUp(0))
        {
            EventMgr.Instance.EventDispatcher("̧��������");
        }
    }
}
