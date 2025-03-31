using UnityEngine;

namespace YSH.Framework
{
    public class SafeAreaAdjuster : MonoBehaviour
    {
        private RectTransform rectTransform;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

#if UNITY_EDITOR
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
            //var info = WeChatWASM.WX.GetSystemInfoSync();

            //Vector2 anchorMin = new Vector2((float)info.safeArea.left, (float)info.safeArea.top);
            //Vector2 anchorMax = new Vector2((float)info.safeArea.right, (float)info.safeArea.bottom);

            //anchorMin.x /= (float)info.screenWidth;
            //anchorMin.y /= (float)info.screenHeight;
            //anchorMax.x /= (float)info.screenWidth;
            //anchorMax.y /= (float)info.screenHeight;

            //rectTransform.anchorMin = anchorMin;
            //rectTransform.anchorMax = anchorMax;
        }
    }
}

