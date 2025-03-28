using System.Collections.Generic;
using UnityEngine;

namespace YSH.Framework.Extensions
{
    public static class TransformExtensions
    {
        /// <summary>
        /// 递归查找当前 Transform 下的子物体，匹配指定名称的物体。
        /// </summary>
        /// <param name="parent">起始查找的父物体。</param>
        /// <param name="childName">要查找的子物体名称。</param>
        /// <returns>找到的子物体的 Transform，如果未找到则返回 null。</returns>
        public static Transform FindChildByNameRecursive(this Transform parent, string childName)
        {
            // 如果当前物体的名称匹配，则返回该物体
            if (parent.name == childName)
                return parent;

            // 遍历所有子物体并递归查找
            foreach (Transform child in parent)
            {
                Transform result = child.FindChildByNameRecursive(childName);
                if (result != null)
                    return result;
            }

            // 如果没有找到匹配的物体，则返回 null
            return null;
        }

        /// <summary>
        /// 在当前 Transform 的层级中查找指定名称的物体（包括兄弟物体、父物体和当前物体）。
        /// </summary>
        /// <param name="current">当前的 Transform 物体。</param>
        /// <param name="name">要查找的物体名称。</param>
        /// <returns>找到的 Transform 物体，如果未找到则返回 null。</returns>
        public static Transform FindTransformInHierarchy(this Transform current, string name)
        {
            // 首先检查当前物体
            if (current.name == name)
            {
                return current;
            }

            // 如果当前物体有父物体，检查它的所有兄弟物体
            if (current.parent != null)
            {
                foreach (Transform sibling in current.parent)
                {
                    if (sibling != current && sibling.name == name)
                    {
                        return sibling;
                    }
                }
            }

            // 继续查找父物体及其兄弟物体
            return FindTransformInParentAndSiblings(current.parent, name);
        }

        /// <summary>
        /// 递归查找父物体及其兄弟物体，直到找到指定名称的物体。
        /// </summary>
        /// <param name="parent">当前父物体。</param>
        /// <param name="name">要查找的物体名称。</param>
        /// <returns>找到的 Transform 物体，如果未找到则返回 null。</returns>
        private static Transform FindTransformInParentAndSiblings(Transform parent, string name)
        {
            while (parent != null)
            {
                // 检查当前父物体
                if (parent.name == name)
                {
                    return parent;
                }

                // 检查父物体的兄弟物体
                if (parent.parent != null)
                {
                    foreach (Transform sibling in parent.parent)
                    {
                        if (sibling != parent && sibling.name == name)
                        {
                            return sibling;
                        }
                    }
                }

                // 向上查找父物体，继续查找
                parent = parent.parent;
            }

            // 如果没有找到匹配的物体，则返回 null
            return null;
        }

        /// <summary>
        /// 查找当前物体下（包括所有子物体和孙子物体）名称匹配的所有物体。
        /// </summary>
        /// <param name="parent">起始查找的父物体。</param>
        /// <param name="targetName">目标物体名称。</param>
        /// <returns>一个包含所有匹配物体的列表。</returns>
        public static List<Transform> FindAllDescendantsByName(this Transform parent, string targetName)
        {
            List<Transform> foundObjects = new List<Transform>();

            // 递归查找所有符合条件的子物体
            FindDescendantsRecursive(parent, targetName, foundObjects);

            return foundObjects;
        }

        /// <summary>
        /// 递归查找所有子物体及其孙子物体，匹配名称的物体将被添加到结果列表。
        /// </summary>
        /// <param name="parent">起始查找的父物体。</param>
        /// <param name="targetName">目标物体名称。</param>
        /// <param name="foundObjects">存储匹配物体的列表。</param>
        private static void FindDescendantsRecursive(Transform parent, string targetName, List<Transform> foundObjects)
        {
            foreach (Transform child in parent)
            {
                // 如果物体名称匹配，则添加到结果列表
                if (child.name == targetName)
                {
                    foundObjects.Add(child.transform);
                }

                // 如果有子物体，递归查找
                if (child.childCount > 0)
                {
                    FindDescendantsRecursive(child, targetName, foundObjects);
                }
            }
        }

        /// <summary>
        /// 获取当前物体在层级中的完整路径（从根物体到当前物体）。
        /// </summary>
        /// <param name="transform">需要获取路径的 Transform 物体。</param>
        /// <returns>物体的层级路径字符串。</returns>
        public static string GetFullHierarchyPath(this Transform transform)
        {
            string path = transform.name;
            while (transform.parent != null)
            {
                transform = transform.parent;
                path = transform.name + "/" + path;
            }
            return path;
        }
    }
}
