using UnityEngine;

namespace YSH.Framework
{
    public class GMMgr : MonoSingleton<GMMgr>
    {
        //是否启用GM
        public bool isEnableGM;

        // 用户输入的命令
        private string inputText = "";
        // 滚动位置
        private Vector2 scrollPosition = Vector2.zero;
        // 是否显示 GM 界面
        private bool showConsole = false;

        private GMCmd gmCmd = new GMCmd();

        private void Update()
        {
            if (!isEnableGM) return;

            // PC 端按 ~ 键打开 GM 面板
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                showConsole = !showConsole;
            }

            // 移动端检测四指点击
            if (Input.touchCount >= 4)
            {
                showConsole = !showConsole;
            }
        }

        private void OnGUI()
        {
            if (!showConsole) return;

            GUIStyle boxStyle = new GUIStyle(GUI.skin.box)
            {
                fontSize = 20,
                normal = { textColor = Color.white },
                alignment = TextAnchor.MiddleLeft
            };

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 24,
                alignment = TextAnchor.MiddleLeft
            };

            GUIStyle inputStyle = new GUIStyle(GUI.skin.textField)
            {
                fontSize = 24
            };

            GUIStyle executeButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 24,
                alignment = TextAnchor.MiddleCenter // 让“执行”按钮的文字居中
            };

            GUIStyle clearButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 24,
                alignment = TextAnchor.MiddleCenter // 让“清空”按钮的文字居中
            };

            // 计算 GUI 高度
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            float inputHeight = 80; // 输入框高度
            float resultHeight = screenHeight - inputHeight; // 结果区域高度

            // 获取模糊匹配结果
            var matchedCommands = gmCmd.GetMatchingCommands(inputText);

            // 开始滚动区域
            scrollPosition = GUI.BeginScrollView(new Rect(0, 0, screenWidth, resultHeight), scrollPosition, new Rect(0, 0, screenWidth - 20, matchedCommands.Count * 90));

            float yOffset = 0;
            foreach (var command in matchedCommands)
            {
                string displayText = $"{command.Name}【{command.Description}】"; // 例如：AddGold【点击可以增加金币】

                if (GUI.Button(new Rect(20, yOffset, screenWidth - 40, 80), displayText, buttonStyle))
                {
                    gmCmd.ExecuteCommand(command.Name); // 点击按钮默认执行命令（无参数）
                    inputText = command.Name;
                }
                yOffset += 90;
            }

            GUI.EndScrollView();

            // 输入框
            inputText = GUI.TextField(new Rect(20, screenHeight - inputHeight, screenWidth - 350, inputHeight - 20), inputText, inputStyle);

            // 执行按钮
            if (GUI.Button(new Rect(screenWidth - 300, screenHeight - inputHeight, 120, inputHeight - 20), "执行", executeButtonStyle))
            {
                gmCmd.ExecuteCommand(inputText);
            }

            // 清空按钮
            if (GUI.Button(new Rect(screenWidth - 150, screenHeight - inputHeight, 120, inputHeight - 20), "清空", clearButtonStyle))
            {
                inputText = ""; // 清空输入框
            }
        }
    }
}

