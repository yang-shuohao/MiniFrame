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


    //查找当前Transform的兄弟和父物体（直到最顶层）是否存在特定名字的物体
    public static Transform FindInHierarchy(this Transform current, string name)
    {
        // 检查当前对象
        if (current.name == name)
        {
            return current;
        }

        // 检查当前对象的兄弟
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

        // 检查父对象及其兄弟
        return FindInParentAndSiblings(current.parent, name);
    }

    private static Transform FindInParentAndSiblings(Transform parent, string name)
    {
        while (parent != null)
        {
            // 检查父对象
            if (parent.name == name)
            {
                return parent;
            }

            // 检查父对象的兄弟
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

            // 移动到父对象，继续向上查找
            parent = parent.parent;
        }

        // 如果没有找到，返回 null
        return null;
    }
}

