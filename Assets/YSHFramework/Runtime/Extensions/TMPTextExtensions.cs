
using DG.Tweening;
using TMPro;

namespace YSH.Framework.Extensions
{
    public static class TMPTextExtensions
    {
        /// <summary>
        /// �� TMP_Text �����������ֵ�� startValue �仯�� endValue��
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
        /// �� TMP_Text ����ĸ������� startValue �仯�� endValue��֧��С������
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
        /// �� TMP_Text ���ʵ�ִ��ֻ�Ч����������ʾ�ı����ݡ�
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

