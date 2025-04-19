
using UnityEngine;
using UnityEngine.UI;

namespace YSH.Framework.Extensions
{
    public static class RectTransformExtensions
    {
        /// <summary>
        /// 为 RectTransform 及其所有子物体的 Image 组件设置材质
        /// </summary>
        /// <param name="rectTransform">目标 RectTransform</param>
        /// <param name="newMaterial">要应用的材质</param>
        public static void ApplyMaterialToImages(this RectTransform rectTransform, Material newMaterial)
        {
            if (rectTransform == null || newMaterial == null) return;

            // 递归遍历并设置材质
            foreach (var image in rectTransform.GetComponentsInChildren<Image>(true))
            {
                image.material = newMaterial;
            }
        }

        /// <summary>
        /// 根据 Sprite 设置 RectTransform 的大小、中心点和位置
        /// </summary>
        /// <param name="rectTransform">目标 RectTransform</param>
        /// <param name="sprite">要应用的 Sprite</param>
        /// <param name="anchoredPosition">可选参数：锚点位置</param>
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


