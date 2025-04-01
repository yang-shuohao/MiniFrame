using UnityEngine;

namespace YSH.Framework
{
    public class SafeAreaAdjuster : MonoBehaviour
    {
        private RectTransform rectTransform;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

#if UNITY_EDITOR || UNITY_STANDALONE
            ApplySafeArea();
#else
        WXApplySafeArea();
#endif
        }

        void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }

        void WXApplySafeArea()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            var info = WeChatWASM.WX.GetSystemInfoSync();

            float safeLeft = (float)info.safeArea.left;
            float safeTop = (float)info.safeArea.top;
            float safeWidth = (float)info.safeArea.width;
            float safeHeight = (float)info.safeArea.height;

            Vector2 anchorMin = new Vector2(safeLeft / (float)info.screenWidth,
                                            safeTop / (float)info.screenHeight);

            //Vector2 anchorMax = new Vector2((safeLeft + safeWidth) / (float)info.screenWidth,
            //                                (safeTop + safeHeight) / (float)info.screenHeight);
            //TODO，右边使用左边一样的安全区
            Vector2 anchorMax = new Vector2(1f - anchorMin.x, 1f - anchorMin.y);

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
#endif
        }
    }
}

