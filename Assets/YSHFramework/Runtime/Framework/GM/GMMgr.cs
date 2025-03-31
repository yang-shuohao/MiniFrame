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
        private Texture2D solidBackgroundTexture;

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

            // 如果纹理还没有创建，则创建一个带透明度的背景纹理
            if (solidBackgroundTexture == null)
            {
                solidBackgroundTexture = new Texture2D(1, 1);
                solidBackgroundTexture.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.8f));
                solidBackgroundTexture.Apply();
            }
            // 绘制一个全屏背景，带透明度
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), solidBackgroundTexture);

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
            if (GUI.Button(new Rect(screenWidth - 300, screenHeight - inputHeight, 120, inputHeight - 20), "Run", executeButtonStyle))
            {
                gmCmd.ExecuteCommand(inputText);
            }

            // 清空按钮
            if (GUI.Button(new Rect(screenWidth - 150, screenHeight - inputHeight, 120, inputHeight - 20), "Clear", clearButtonStyle))
            {
                inputText = ""; // 清空输入框
            }
        }
    }
}

