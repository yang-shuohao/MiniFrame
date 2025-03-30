
using DG.Tweening;
using TMPro;

namespace YSH.Framework.Extensions
{
    public static class TMPTextExtensions
    {
        /// <summary>
        /// 让 TMP_Text 组件的整数数值从 startValue 变化到 endValue。
        /// </summary>
        public static Tweener DOCountInt(this TMP_Text tmpText, int startValue, int endValue, float duration)
        {
            return DOTween.To(() => startValue, x =>
            {
                startValue = x;
                tmpText.text = startValue.ToString();
            }, endValue, duration).SetEase(Ease.Linear);
        }

        /// <summary>
        /// 让 TMP_Text 组件的浮点数从 startValue 变化到 endValue（支持小数）。
        /// </summary>
        public static Tweener DOCountFloat(this TMP_Text tmpText, float startValue, float endValue, float duration, int decimalPlaces = 2)
        {
            return DOTween.To(() => startValue, x =>
            {
                startValue = x;
                tmpText.text = startValue.ToString($"F{decimalPlaces}");
            }, endValue, duration).SetEase(Ease.Linear);
        }

        /// <summary>
        /// 让 TMP_Text 组件实现打字机效果，逐字显示文本内容。
        /// </summary>
        public static Tweener DOTyping(this TMP_Text tmpText, string fullText, float duration, System.Action onComplete = null)
        {
            int totalLength = fullText.Length;

            tmpText.text = fullText;
            tmpText.maxVisibleCharacters = 0;

            return DOTween.To(() => 0, x =>
            {
                tmpText.maxVisibleCharacters = x;
            }, totalLength, duration)
            .OnComplete(()=>
            {
                onComplete?.Invoke();
            })
            .SetEase(Ease.Linear);
        }
    }
}

