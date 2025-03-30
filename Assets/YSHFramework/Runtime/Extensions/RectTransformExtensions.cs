
using UnityEngine;
using UnityEngine.UI;

namespace YSH.Framework.Extensions
{
    public static class RectTransformExtensions
    {
        /// <summary>
        /// Ϊ RectTransform ��������������� Image ������ò���
        /// </summary>
        /// <param name="rectTransform">Ŀ�� RectTransform</param>
        /// <param name="newMaterial">ҪӦ�õĲ���</param>
        public static void ApplyMaterialToImages(this RectTransform rectTransform, Material newMaterial)
        {
            if (rectTransform == null || newMaterial == null) return;

            // �ݹ���������ò���
            foreach (var image in rectTransform.GetComponentsInChildren<Image>(true))
            {
                image.material = newMaterial;
            }
        }

        /// <summary>
        /// ���� Sprite ���� RectTransform �Ĵ�С�����ĵ��λ��
        /// </summary>
        /// <param name="rectTransform">Ŀ�� RectTransform</param>
        /// <param name="sprite">ҪӦ�õ� Sprite</param>
        /// <param name="anchoredPosition">��ѡ������ê��λ��</param>
        public static void ApplySpriteProperties(this RectTransform rectTransform, Sprite sprite, Vector2? anchoredPosition = null)
        {
            if (rectTransform == null || sprite == null) return;

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sprite.rect.width);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sprite.rect.height);
            rectTransform.pivot = sprite.pivot / sprite.rect.size;

            if (anchoredPosition.HasValue)
            {
                rectTransform.anchoredPosition = anchoredPosition.Value;
            }
        }
    }
}


