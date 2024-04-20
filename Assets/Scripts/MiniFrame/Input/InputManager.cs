using UnityEngine;

/// <summary>
/// ���������
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
        if(IsEnableInput)
        {
            CheckInput();
        }
    }

    private void CheckInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            EventManager.Instance.EventDispatcher("����������");
        }
        else if(Input.GetMouseButton(0))
        {
            EventManager.Instance.EventDispatcher("��ס������");
        }
        else if(Input.GetMouseButtonUp(0))
        {
            EventManager.Instance.EventDispatcher("̧��������");
        }
    }
}
