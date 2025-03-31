
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
            // ȡ��֮ǰ������Э�̣���ֹ��ε���ʱ�ص���
            if (hideCoroutine != null)
            {
                StopCoroutine(hideCoroutine);
            }

            message = msg;
            showPopup = true;

            // �����µ�����Э��
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
                // ����depth��ȷ����ʾ������UIԪ����
                GUI.depth = -int.MaxValue;

                // ��OnGUI�г�ʼ��GUIStyle
                GUIStyle style = new GUIStyle(GUI.skin.box)
                {
                    fontSize = 50,
                    alignment = TextAnchor.MiddleCenter
                };
                style.normal.textColor = Color.red; // ����������ɫΪ��ɫ

                // �������λ��
                int width = 300;
                int height = 100;

                // ���ñ�����ɫ
                style.normal.background = MakeTexture(width, height, Color.green); // ���ñ���Ϊ��ɫ

                Rect rect = new Rect((Screen.width - width) / 2, Screen.height / 3, width, height);

                // ʹ��GUIStyle��Ⱦ��ɫ�ı�
                GUI.Box(rect, message, style);
            }
        }

        // ����һ��ָ����ɫ�� 1x1 ��������
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

