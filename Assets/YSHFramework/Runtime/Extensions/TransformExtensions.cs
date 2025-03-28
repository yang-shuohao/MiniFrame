using System.Collections.Generic;
using UnityEngine;

namespace YSH.Framework.Extensions
{
    public static class TransformExtensions
    {
        /// <summary>
        /// �ݹ���ҵ�ǰ Transform �µ������壬ƥ��ָ�����Ƶ����塣
        /// </summary>
        /// <param name="parent">��ʼ���ҵĸ����塣</param>
        /// <param name="childName">Ҫ���ҵ����������ơ�</param>
        /// <returns>�ҵ���������� Transform�����δ�ҵ��򷵻� null��</returns>
        public static Transform FindChildByNameRecursive(this Transform parent, string childName)
        {
            // �����ǰ���������ƥ�䣬�򷵻ظ�����
            if (parent.name == childName)
                return parent;

            // �������������岢�ݹ����
            foreach (Transform child in parent)
            {
                Transform result = child.FindChildByNameRecursive(childName);
                if (result != null)
                    return result;
            }

            // ���û���ҵ�ƥ������壬�򷵻� null
            return null;
        }

        /// <summary>
        /// �ڵ�ǰ Transform �Ĳ㼶�в���ָ�����Ƶ����壨�����ֵ����塢������͵�ǰ���壩��
        /// </summary>
        /// <param name="current">��ǰ�� Transform ���塣</param>
        /// <param name="name">Ҫ���ҵ��������ơ�</param>
        /// <returns>�ҵ��� Transform ���壬���δ�ҵ��򷵻� null��</returns>
        public static Transform FindTransformInHierarchy(this Transform current, string name)
        {
            // ���ȼ�鵱ǰ����
            if (current.name == name)
            {
                return current;
            }

            // �����ǰ�����и����壬������������ֵ�����
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

            // �������Ҹ����弰���ֵ�����
            return FindTransformInParentAndSiblings(current.parent, name);
        }

        /// <summary>
        /// �ݹ���Ҹ����弰���ֵ����壬ֱ���ҵ�ָ�����Ƶ����塣
        /// </summary>
        /// <param name="parent">��ǰ�����塣</param>
        /// <param name="name">Ҫ���ҵ��������ơ�</param>
        /// <returns>�ҵ��� Transform ���壬���δ�ҵ��򷵻� null��</returns>
        private static Transform FindTransformInParentAndSiblings(Transform parent, string name)
        {
            while (parent != null)
            {
                // ��鵱ǰ������
                if (parent.name == name)
                {
                    return parent;
                }

                // ��鸸������ֵ�����
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

                // ���ϲ��Ҹ����壬��������
                parent = parent.parent;
            }

            // ���û���ҵ�ƥ������壬�򷵻� null
            return null;
        }

        /// <summary>
        /// ���ҵ�ǰ�����£�����������������������壩����ƥ����������塣
        /// </summary>
        /// <param name="parent">��ʼ���ҵĸ����塣</param>
        /// <param name="targetName">Ŀ���������ơ�</param>
        /// <returns>һ����������ƥ��������б�</returns>
        public static List<Transform> FindAllDescendantsByName(this Transform parent, string targetName)
        {
            List<Transform> foundObjects = new List<Transform>();

            // �ݹ�������з���������������
            FindDescendantsRecursive(parent, targetName, foundObjects);

            return foundObjects;
        }

        /// <summary>
        /// �ݹ�������������弰���������壬ƥ�����Ƶ����彫����ӵ�����б�
        /// </summary>
        /// <param name="parent">��ʼ���ҵĸ����塣</param>
        /// <param name="targetName">Ŀ���������ơ�</param>
        /// <param name="foundObjects">�洢ƥ��������б�</param>
        private static void FindDescendantsRecursive(Transform parent, string targetName, List<Transform> foundObjects)
        {
            foreach (Transform child in parent)
            {
                // �����������ƥ�䣬����ӵ�����б�
                if (child.name == targetName)
                {
                    foundObjects.Add(child.transform);
                }

                // ����������壬�ݹ����
                if (child.childCount > 0)
                {
                    FindDescendantsRecursive(child, targetName, foundObjects);
                }
            }
        }

        /// <summary>
        /// ��ȡ��ǰ�����ڲ㼶�е�����·�����Ӹ����嵽��ǰ���壩��
        /// </summary>
        /// <param name="transform">��Ҫ��ȡ·���� Transform ���塣</param>
        /// <returns>����Ĳ㼶·���ַ�����</returns>
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
