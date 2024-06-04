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
}
