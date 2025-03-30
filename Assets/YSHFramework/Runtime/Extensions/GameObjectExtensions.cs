using UnityEngine;

namespace YSH.Framework.Extensions
{
    /// <summary>
    /// 递归设置 GameObject 及其所有子对象的层级。
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// 递归设置 GameObject 及其所有子对象的层级。
        /// </summary>
        /// <param name="gameObject">需要更改层级的 GameObject。</param>
        /// <param name="layer">目标层级。</param>
        public static void SetLayerRecursive(this GameObject gameObject, int layer)
        {
            if (gameObject == null) return;

            // 如果当前层级已经是目标层级，则跳过，避免不必要的赋值
            if (gameObject.layer != layer)
            {
                gameObject.layer = layer;
            }

            // 使用 Transform 遍历所有子物体，减少 GetComponent 调用
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetLayerRecursive(layer);
            }
        }
    }
}