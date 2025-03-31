
using System.Collections;
using UnityEngine;

namespace YSH.Framework
{
    public class GUIMgr : MonoSingleton<GUIMgr>
    {
        private bool showPopup = false;
        private string message = "";
        private Coroutine hideCoroutine;

        public void ShowPopup(string msg, float duration = 2f)
        {
            // 取消之前的隐藏协程（防止多次调用时重叠）
            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
            }

            message = msg;
            showPopup = true;

            // 启动新的隐藏协程
            hideCoroutine = StartCoroutine(HideAfterTime(duration));
        }

        private IEnumerator HideAfterTime(float duration)
        {
            yield return new WaitForSeconds(duration);
            showPopup = false;
        }

        private void OnGUI()
        {
            if (showPopup)
            {
                // 设置depth，确保显示在其他UI元素上
                GUI.depth = -int.MaxValue;

                // 在OnGUI中初始化GUIStyle
                GUIStyle style = new GUIStyle(GUI.skin.box)
                {
                    fontSize = 50,
                    alignment = TextAnchor.MiddleCenter
                };
                style.normal.textColor = Color.red; // 设置字体颜色为红色

                // 计算居中位置
                int width = 300;
                int height = 100;

                // 设置背景颜色
                style.normal.background = MakeTexture(width, height, Color.green); // 设置背景为绿色

                Rect rect = new Rect((Screen.width - width) / 2, Screen.height / 3, width, height);

                // 使用GUIStyle渲染红色文本
                GUI.Box(rect, message, style);
            }
        }

        // 创建一个指定颜色的 1x1 像素纹理
        public Texture2D MakeTexture(int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(width, height);
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
    }
}

