
using UnityEngine;

public class GMGUIMgr : MonoSingleton<GMGUIMgr>
{
    private string logContent = "";
    private Vector2 logScrollPosition;
    private string commandInput = "";

    public bool isShowGMUI;

    protected override void Awake()
    {
        base.Awake();

        isShowGMUI = true;
    }

    public void OnGUI()
    {
        if(isShowGMUI)
        {
            ShowGMUI();
        }
    }

    protected void ShowGMUI()
    {
        // ���û�����ʽ
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 16
        };
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 14
        };
        GUIStyle textFieldStyle = new GUIStyle(GUI.skin.textField)
        {
            fontSize = 14
        };
        GUIStyle textAreaStyle = new GUIStyle(GUI.skin.textArea)
        {
            fontSize = 14
        };

        // ���ϽǵĹرհ�ť
        if (GUI.Button(new Rect(Screen.width - 110, 10, 100, 30), "�ر�", buttonStyle))
        {
            isShowGMUI = false;
        }

        // �м䲿����ʾ��־
        GUI.Box(new Rect(10, 10, Screen.width - 130, Screen.height - 100), "��־");
        logScrollPosition = GUI.BeginScrollView(new Rect(20, 40, Screen.width - 150, Screen.height - 150), logScrollPosition, new Rect(0, 0, Screen.width - 170, logContent.Length * 14), false, false);
        GUI.TextArea(new Rect(0, 0, Screen.width - 170, logContent.Length * 14), logContent, textAreaStyle);
        GUI.EndScrollView();

        // ���½�
        GUI.Label(new Rect(10, Screen.height - 80, 80, 30), "������ָ��", labelStyle);

        // �����м���ʾ�����
        commandInput = GUI.TextField(new Rect(100, Screen.height - 80, Screen.width - 280, 30), commandInput, textFieldStyle);

        // �����ұ���ʾ�ύ��ť
        if (GUI.Button(new Rect(Screen.width - 160, Screen.height - 80, 150, 30), "�ύ", buttonStyle))
        {
            //ִ������
            GMCommandMgr.Instance.ExecuteCommand(commandInput);

            // �ύ������߼�
            LogMgr.Instance.Log("�ύ��ť�������������" + commandInput);
            // �������������ӵ���־��
            logContent += "\n" + commandInput;
            // ��������
            commandInput = "";
        }
    }

}
