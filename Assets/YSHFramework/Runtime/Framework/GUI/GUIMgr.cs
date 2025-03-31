
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
                // ����depth��ȷ����ʾ������UIԪ����
                GUI.depth = -int.MaxValue;

                // ��OnGUI�г�ʼ��GUIStyle
                GUIStyle style = new GUIStyle(GUI.skin.box)
                {
                    fontSize = 50,
                    alignment = TextAnchor.MiddleCenter
                };
                style.normal.textColor = Color.red; // ����������ɫΪ��ɫ

                // ���ñ�����ɫ
                style.normal.background = CreateTexture(Color.green); // ���ñ���Ϊ��ɫ

                // �������λ��
                float width = 300;
                float height = 100;
                Rect rect = new Rect((Screen.width - width) / 2, Screen.height / 3, width, height);

                // ʹ��GUIStyle��Ⱦ��ɫ�ı�
                GUI.Box(rect, message, style);
            }
        }

        // ����һ��ָ����ɫ�� 1x1 ��������
        private Texture2D CreateTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
    }
}

