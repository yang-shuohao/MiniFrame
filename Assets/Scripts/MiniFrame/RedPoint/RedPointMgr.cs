using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public class RedPointMgr : Singleton<RedPointMgr>
{
    /// <summary>
    /// ���нڵ㼯��
    /// </summary>
    private Dictionary<string, TreeNode> allNodes;

    /// <summary>
    /// ��ڵ㼯��
    /// </summary>
    private HashSet<TreeNode> dirtyNodes;

    /// <summary>
    /// ��ʱ��ڵ㼯��
    /// </summary>

    private List<TreeNode> tempDirtyNodes;

    /// <summary>
    /// �ڵ������ı�ص�
    /// </summary>
    public Action NodeNumChangeCallback;

    /// <summary>
    /// �ڵ�ֵ�ı�ص�
    /// </summary>
    public Action<TreeNode, int> NodeValueChangeCallback;

    /// <summary>
    /// ·���ָ��ַ�
    /// </summary>
    public char SplitChar
    {
        get;
        private set;
    }

    /// <summary>
    /// �����StringBuild
    /// </summary>
    public StringBuilder CachedSb
    {
        get;
        private set;
    }

    /// <summary>
    /// ��������ڵ�
    /// </summary>
    public TreeNode Root
    {
        get;
        private set;
    }


    public RedPointMgr()
    {
        SplitChar = '/';
        allNodes = new Dictionary<string, TreeNode>();
        Root = new TreeNode("Root");
        dirtyNodes = new HashSet<TreeNode>();
        tempDirtyNodes = new List<TreeNode>();
        CachedSb = new StringBuilder();
    }

    /// <summary>
    /// ��ӽڵ�ֵ����
    /// </summary>
    public TreeNode AddListener(string path, Action<int> callback)
    {
        if (callback == null)
        {
            return null;
        }

        TreeNode node = GetTreeNode(path);
        node.AddListener(callback);

        return node;
    }

    /// <summary>
    /// �Ƴ��ڵ�ֵ����
    /// </summary>
    public void RemoveListener(string path, Action<int> callback)
    {
        if (callback == null)
        {
            return;
        }

        TreeNode node = GetTreeNode(path);
        node.RemoveListener(callback);
    }

    /// <summary>
    /// �Ƴ����нڵ�ֵ����
    /// </summary>
    public void RemoveAllListener(string path)
    {
        TreeNode node = GetTreeNode(path);
        node.RemoveAllListener();
    }

    /// <summary>
    /// �ı�ڵ�ֵ
    /// </summary>
    public void ChangeValue(string path, int newValue)
    {
        TreeNode node = GetTreeNode(path);
        node.ChangeValue(newValue);
    }

    /// <summary>
    /// ��ȡ�ڵ�ֵ
    /// </summary>
    public int GetValue(string path)
    {
        TreeNode node = GetTreeNode(path);
        if (node == null)
        {
            return 0;
        }

        return node.Value;
    }

    /// <summary>
    /// ��ȡ�ڵ�
    /// </summary>
    public TreeNode GetTreeNode(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new Exception("·�����Ϸ�������Ϊ��");
        }

        if (allNodes.TryGetValue(path, out TreeNode node))
        {
            return node;
        }

        TreeNode cur = Root;
        int length = path.Length;

        int startIndex = 0;

        for (int i = 0; i < length; i++)
        {
            if (path[i] == SplitChar)
            {
                if (i == length - 1)
                {
                    throw new Exception("·�����Ϸ���������·���ָ�����β��" + path);
                }

                int endIndex = i - 1;
                if (endIndex < startIndex)
                {
                    throw new Exception("·�����Ϸ������ܴ���������·���ָ�������·���ָ�����ͷ��" + path);
                }

                TreeNode child = cur.GetOrAddChild(new RangeString(path, startIndex, endIndex));

                //����startIndex
                startIndex = i + 1;

                cur = child;
            }
        }

        //���һ���ڵ� ֱ����length - 1��ΪendIndex
        TreeNode target = cur.GetOrAddChild(new RangeString(path, startIndex, length - 1));

        allNodes.Add(path, target);

        return target;


    }

    /// <summary>
    /// �Ƴ��ڵ�
    /// </summary>
    public bool RemoveTreeNode(string path)
    {
        if (!allNodes.ContainsKey(path))
        {
            return false;
        }

        TreeNode node = GetTreeNode(path);
        allNodes.Remove(path);
        return node.Parent.RemoveChild(new RangeString(node.Name, 0, node.Name.Length - 1));
    }

    /// <summary>
    /// �Ƴ����нڵ�
    /// </summary>
    public void RemoveAllTreeNode()
    {
        Root.RemoveAllChild();
        allNodes.Clear();
    }

    /// <summary>
    /// ��������ѯ
    /// </summary>
    public void Update()
    {
        if (dirtyNodes.Count == 0)
        {
            return;
        }

        tempDirtyNodes.Clear();
        foreach (TreeNode node in dirtyNodes)
        {
            tempDirtyNodes.Add(node);
        }
        dirtyNodes.Clear();

        //����������ڵ�
        for (int i = 0; i < tempDirtyNodes.Count; i++)
        {
            tempDirtyNodes[i].ChangeValue();
        }
    }

    /// <summary>
    /// �����ڵ�
    /// </summary>
    public void MarkDirtyNode(TreeNode node)
    {
        if (node == null || node.Name == Root.Name)
        {
            return;
        }

        dirtyNodes.Add(node);
    }

}
