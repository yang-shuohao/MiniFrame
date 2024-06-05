using UnityEngine;

public static class TransformExtensions
{
    // �ݹ����������
    public static Transform FindChildRecursive(this Transform parent, string childName)
    {
        // ���������Ϊ�������ƣ��򷵻ظö���
        if (parent.name == childName)
            return parent;

        // ���������Ӷ��󲢲����������ƵĶ���
        foreach (Transform child in parent)
        {
            Transform result = child.FindChildRecursive(childName);
            if (result != null)
                return result;
        }

        // ���δ�ҵ��κ�ƥ����򷵻�null
        return null;
    }

    // �ҵ���������
    public static Transform FindTopmostParent(this Transform currentTransform)
    {
        Transform topmostParent = currentTransform;

        while (topmostParent.parent != null)
        {
            topmostParent = topmostParent.parent;
        }

        return topmostParent;
    }


    //���ҵ�ǰTransform���ֵܺ͸����壨ֱ����㣩�Ƿ�����ض����ֵ�����
    public static Transform FindInHierarchy(this Transform current, string name)
    {
        // ��鵱ǰ����
        if (current.name == name)
        {
            return current;
        }

        // ��鵱ǰ������ֵ�
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

        // ��鸸�������ֵ�
        return FindInParentAndSiblings(current.parent, name);
    }

    private static Transform FindInParentAndSiblings(Transform parent, string name)
    {
        while (parent != null)
        {
            // ��鸸����
            if (parent.name == name)
            {
                return parent;
            }

            // ��鸸������ֵ�
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

            // �ƶ��������󣬼������ϲ���
            parent = parent.parent;
        }

        // ���û���ҵ������� null
        return null;
    }
}

