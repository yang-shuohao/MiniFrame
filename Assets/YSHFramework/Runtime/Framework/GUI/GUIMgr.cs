
using System.Collections;
using UnityEngine;
using YSH.Framework.Utils;

namespace YSH.Framework
{
    public class GUIMgr : MonoSingleton<GUIMgr>
    {
        private bool showPopup = false;
        private string message = "";

        public void ShowPopup(string msg, float duration = 2f)
        {
            message = msg;
            showPopup = true;
            StartCoroutine(HideAfterTime(duration));
        }

        private IEnumerator HideAfterTime(float duration)
        {
            yield return CoroutineCache.GetWaitForSeconds(duration);
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

                // 设置背景颜色
                style.normal.background = CreateTexture(Color.green); // 设置背景为蓝色

                // 计算居中位置
                float width = 300;
                float height = 100;
                Rect rect = new Rect((Screen.width - width) / 2, Screen.height / 3, width, height);

                // 使用GUIStyle渲染红色文本
                GUI.Box(rect, message, style);
            }
        }

        // 创建一个指定颜色的 1x1 像素纹理
        private Texture2D CreateTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
    }
}

