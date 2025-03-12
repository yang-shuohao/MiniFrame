using UnityEngine;

namespace YSH.Framework
{
    public class GMGUIMgr : MonoSingleton<GMGUIMgr>
    {
        private string logContent = "";
        private Vector2 logScrollPosition;
        private string commandInput = "";
        private string[] matchingCommands = new string[0];
        private Vector2 commandListScrollPosition;

        public bool isShowGMUI;

        private void Awake()
        {
            isShowGMUI = true;
        }

        private void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            // PC 平台
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                //按下波浪键
                isShowGMUI = true;
            }
#elif UNITY_ANDROID || UNITY_IOS
        // 移动平台
        if (Input.touchCount == 4)
        {
            bool allFingersDown = true;
            foreach (Touch touch in Input.touches)
            {
                if (!touch.phase.Equals(TouchPhase.Began) && !touch.phase.Equals(TouchPhase.Moved) && !touch.phase.Equals(TouchPhase.Stationary))
                {
                    allFingersDown = false;
                    break;
                }
            }

            if (allFingersDown)
            {
                //四个手指同时按下
                isShowGMUI = true;
            }
        }
#endif


        }

        public void OnGUI()
        {
            if (isShowGMUI)
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

            // 中间部分显示日志
            GUI.Box(new Rect(10, 10, Screen.width - 20, Screen.height - 100), "日志");
            logScrollPosition = GUI.BeginScrollView(new Rect(20, 40, Screen.width - 40, Screen.height - 150), logScrollPosition, new Rect(0, 0, Screen.width - 60, logContent.Length * 14), false, false);
            GUI.TextArea(new Rect(0, 0, Screen.width - 60, logContent.Length * 14), logContent, textAreaStyle);
            GUI.EndScrollView();

            // 左下角显示输入提示
            GUI.Label(new Rect(10, Screen.height - 80, 80, 30), "请输入指令", labelStyle);

            // 下面中间显示输入框
            commandInput = GUI.TextField(new Rect(100, Screen.height - 80, Screen.width - 320, 30), commandInput, textFieldStyle);

            // 显示匹配的命令列表
            if (!string.IsNullOrEmpty(commandInput))
            {
                matchingCommands = GMCommandMgr.Instance.GetMatchingCommands(commandInput);
            }
            else
            {
                matchingCommands = new string[0];
            }

            // 滚动视图显示匹配的方法列表
            float commandListHeight = Mathf.Min(matchingCommands.Length * 30, 150);
            commandListScrollPosition = GUI.BeginScrollView(new Rect((Screen.width - 400) / 2, Screen.height - 220 - commandListHeight, Screen.width - 100, commandListHeight), commandListScrollPosition, new Rect(0, 0, Screen.width - 100, matchingCommands.Length * 30));
            for (int i = 0; i < matchingCommands.Length; i++)
            {
                if (GUI.Button(new Rect(0, i * 30, 180, 30), matchingCommands[i], buttonStyle))
                {
                    commandInput = matchingCommands[i];
                }
                string description = GMCommandMgr.Instance.GetCommandDescription(matchingCommands[i]);
                GUI.Label(new Rect(200, i * 30, 300, 30), description, labelStyle);
            }
            GUI.EndScrollView();

            // 下面右边显示提交按钮
            if (GUI.Button(new Rect(Screen.width - 200, Screen.height - 80, 80, 30), "提交", buttonStyle))
            {
                if (!string.IsNullOrEmpty(commandInput))
                {
                    // 执行命令
                    GMCommandMgr.Instance.ExecuteCommand(commandInput);
                    // 提交命令处理逻辑
                    LogMgr.Instance.Log("提交按钮点击，输入的命令：" + commandInput);
                    // 将输入的命令添加到日志中
                    logContent += "\n" + commandInput;
                    // 清空输入框
                    commandInput = "";
                }
            }

            // 右下角的关闭按钮
            if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 80, 80, 30), "关闭", buttonStyle))
            {
                isShowGMUI = false;
            }
        }
    }
}

