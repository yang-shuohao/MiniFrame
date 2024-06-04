using UnityEngine;

public static class TransformExtensions
{
    // 递归查找子物体
    public static Transform FindChildRecursive(this Transform parent, string childName)
    {
        // 如果父对象为所需名称，则返回该对象
        if (parent.name == childName)
            return parent;

        // 遍历所有子对象并查找所需名称的对象
        foreach (Transform child in parent)
        {
            Transform result = child.FindChildRecursive(childName);
            if (result != null)
                return result;
        }

        // 如果未找到任何匹配项，则返回null
        return null;
    }

    // 找到顶级父类
    public static Transform FindTopmostParent(this Transform currentTransform)
    {
        Transform topmostParent = currentTransform;

        while (topmostParent.parent != null)
        {
            topmostParent = topmostParent.parent;
        }

        return topmostParent;
    }
}
