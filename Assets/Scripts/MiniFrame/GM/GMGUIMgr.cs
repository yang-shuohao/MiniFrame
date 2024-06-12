
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
        // 设置基本样式
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

        // 右上角的关闭按钮
        if (GUI.Button(new Rect(Screen.width - 110, 10, 100, 30), "关闭", buttonStyle))
        {
            isShowGMUI = false;
        }

        // 中间部分显示日志
        GUI.Box(new Rect(10, 10, Screen.width - 130, Screen.height - 100), "日志");
        logScrollPosition = GUI.BeginScrollView(new Rect(20, 40, Screen.width - 150, Screen.height - 150), logScrollPosition, new Rect(0, 0, Screen.width - 170, logContent.Length * 14), false, false);
        GUI.TextArea(new Rect(0, 0, Screen.width - 170, logContent.Length * 14), logContent, textAreaStyle);
        GUI.EndScrollView();

        // 左下角
        GUI.Label(new Rect(10, Screen.height - 80, 80, 30), "请输入指令", labelStyle);

        // 下面中间显示输入框
        commandInput = GUI.TextField(new Rect(100, Screen.height - 80, Screen.width - 280, 30), commandInput, textFieldStyle);

        // 下面右边显示提交按钮
        if (GUI.Button(new Rect(Screen.width - 160, Screen.height - 80, 150, 30), "提交", buttonStyle))
        {
            //执行命令
            GMCommandMgr.Instance.ExecuteCommand(commandInput);

            // 提交命令处理逻辑
            LogMgr.Instance.Log("提交按钮点击，输入的命令：" + commandInput);
            // 将输入的命令添加到日志中
            logContent += "\n" + commandInput;
            // 清空输入框
            commandInput = "";
        }
    }

}
