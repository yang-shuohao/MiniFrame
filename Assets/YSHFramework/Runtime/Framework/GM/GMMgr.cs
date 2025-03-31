using UnityEngine;

namespace YSH.Framework
{
    public class GMMgr : MonoSingleton<GMMgr>
    {
        // 用户输入的命令
        private string inputText = "";
        // 是否显示 GM 界面
        private bool showConsole = false;

        private GMCmd gmCmd = new GMCmd();

        // 滚动位置
        private Vector2 scrollPosition = Vector2.zero;

        //启用GM
        public void EnableGM()
        {

        }

        private void Update()
        {
            // PC 端按 ~ 键打开 GM 面板
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                showConsole = !showConsole;
                InputMgr.Instance.IsEnableInput = !showConsole;
            }

            // 移动端检测四指点击
            if (Input.touchCount >= 4)
            {
                showConsole = !showConsole;
                InputMgr.Instance.IsEnableInput = !showConsole;
            }
        }

        private void OnGUI()
        {
            if (!showConsole) return;

            // 绘制一个全屏背景，带透明度
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), GUIMgr.Instance.MakeTexture(1, 1, new Color(0f, 0f, 0f, 0.8f)));

            // **按钮样式**
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 40, // 文字更大
                alignment = TextAnchor.MiddleLeft
            };

            // **输入框样式**
            GUIStyle inputStyle = new GUIStyle(GUI.skin.textField)
            {
                fontSize = 40 // 文字更大
            };

            // **执行按钮样式**
            GUIStyle executeButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 40, // 文字更大
                alignment = TextAnchor.MiddleCenter
            };

            // **清空按钮样式**
            GUIStyle clearButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 40, // 文字更大
                alignment = TextAnchor.MiddleCenter
            };

            // **修改滚动条样式**
            GUI.skin.verticalScrollbar.fixedWidth = 40; // **滚动条变宽**
            GUI.skin.verticalScrollbarThumb.fixedWidth = 40; // **滑块（thumb）也变宽**

            // 自定义滑块样式
            GUIStyle thumbStyle = new GUIStyle(GUI.skin.verticalScrollbarThumb)
            {
                fixedWidth = 40, // 设置滑块的宽度为40
                normal = { background = GUIMgr.Instance.MakeTexture(40, 1, Color.green) } // 设置滑块颜色
            };

            // 设置滚动条轨道样式
            GUIStyle trackStyle = new GUIStyle(GUI.skin.verticalScrollbar)
            {
                fixedWidth = 40, // 设置滚动条轨道的宽度为40
                normal = { background = GUIMgr.Instance.MakeTexture(40, 1, Color.gray) } // 设置轨道颜色
            };

            // 获取模糊匹配结果
            var matchedCommands = gmCmd.GetMatchingCommands(inputText);

            // **计算 GUI 高度**
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            float inputHeight = 80; // **输入框高度不变**
            float resultHeight = screenHeight - inputHeight; // **结果区域高度**
            float scrollContentHeight = matchedCommands.Count * 90; // **内容区域高度**

            // **滚动区域**
            scrollPosition = GUI.BeginScrollView(
                new Rect(0, 0, screenWidth, resultHeight),
                scrollPosition,
                new Rect(0, 0, screenWidth - 20, scrollContentHeight), // 保持按钮大小不变
                GUIStyle.none, // **去掉水平滚动条**
                GUI.skin.verticalScrollbar // **使用修改后的滚动条**
            );

            // 设置滚动条样式
            GUI.skin.verticalScrollbarThumb = thumbStyle;
            GUI.skin.verticalScrollbar = trackStyle;

            float yOffset = 0;
            foreach (var command in matchedCommands)
            {
                string displayText = $"{command.Name}【{command.Description}】";

                if (GUI.Button(new Rect(20, yOffset, screenWidth - 40, 80), displayText, buttonStyle)) // 按钮大小不变
                {
                    gmCmd.ExecuteCommand(command.Name);
                    inputText = command.Name;
                }
                yOffset += 90;
            }

            GUI.EndScrollView();

            // **输入框**
            inputText = GUI.TextField(new Rect(20, screenHeight - inputHeight, screenWidth - 350, inputHeight - 20), inputText, inputStyle);

            // **执行按钮**
            if (GUI.Button(new Rect(screenWidth - 300, screenHeight - inputHeight, 120, inputHeight - 20), "Run", executeButtonStyle))
            {
                gmCmd.ExecuteCommand(inputText);
            }

            // **清空按钮**
            if (GUI.Button(new Rect(screenWidth - 150, screenHeight - inputHeight, 120, inputHeight - 20), "Clear", clearButtonStyle))
            {
                inputText = "";
            }
        }
    }
}

