using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    /// <summary>
    /// �ӽڵ�
    /// </summary>
    private Dictionary<RangeString, TreeNode> children;

    /// <summary>
    /// �ڵ�ֵ�ı�ص�
    /// </summary>
    private Action<int> changeCallback;

    /// <summary>
    /// ����·��
    /// </summary>
    private string fullPath;

    /// <summary>
    /// �ڵ���
    /// </summary>
    public string Name
    {
        get;
        private set;
    }

    /// <summary>
    /// ����·��
    /// </summary>
    public string FullPath
    {
        get
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                if (Parent == null || Parent == RedPointMgr.Instance.Root)
                {
                    fullPath = Name;
                }
                else
                {
                    fullPath = Parent.FullPath + RedPointMgr.Instance.SplitChar + Name;
                }
            }

            return fullPath;
        }
    }

    /// <summary>
    /// �ڵ�ֵ
    /// </summary>
    public int Value
    {
        get;
        private set;
    }

    /// <summary>
    /// ���ڵ�
    /// </summary>
    public TreeNode Parent
    {
        get;
        private set;
    }

    /// <summary>
    /// �ӽڵ�
    /// </summary>
    public Dictionary<RangeString, TreeNode>.ValueCollection Children
    {
        get
        {
            return children?.Values;
        }
    }

    /// <summary>
    /// �ӽڵ�����
    /// </summary>
    public int ChildrenCount
    {
        get
        {
            if (children == null)
            {
                return 0;
            }

            int sum = children.Count;
            foreach (TreeNode node in children.Values)
            {
                sum += node.ChildrenCount;
            }
            return sum;
        }
    }

    public TreeNode(string name)
    {
        Name = name;
        Value = 0;
        changeCallback = null;
    }

    public TreeNode(string name, TreeNode parent) : this(name)
    {
        Parent = parent;
    }

    /// <summary>
    /// ��ӽڵ�ֵ����
    /// </summary>
    public void AddListener(Action<int> callback)
    {
        changeCallback += callback;
    }

    /// <summary>
    /// �Ƴ��ڵ�ֵ����
    /// </summary>
    public void RemoveListener(Action<int> callback)
    {
        changeCallback -= callback;
    }

    /// <summary>
    /// �Ƴ����нڵ�ֵ����
    /// </summary>
    public void RemoveAllListener()
    {
        changeCallback = null;
    }

    /// <summary>
    /// �ı�ڵ�ֵ��ʹ�ô������ֵ��ֻ����Ҷ�ӽڵ��ϵ��ã�
    /// </summary>
    public void ChangeValue(int newValue)
    {
        if (children != null && children.Count != 0)
        {
            throw new Exception("������ֱ�Ӹı��Ҷ�ӽڵ��ֵ��" + FullPath);
        }

        InternalChangeValue(newValue);
    }

    /// <summary>
    /// �ı�ڵ�ֵ�������ӽڵ�ֵ������ֵ��ֻ�Է�Ҷ�ӽڵ���Ч��
    /// </summary>
    public void ChangeValue()
    {
        int sum = 0;

        if (children != null && children.Count != 0)
        {
            foreach (KeyValuePair<RangeString, TreeNode> child in children)
            {
                sum += child.Value.Value;
            }
        }

        InternalChangeValue(sum);
    }

    /// <summary>
    /// ��ȡ�ӽڵ㣬��������������
    /// </summary>
    public TreeNode GetOrAddChild(RangeString key)
    {
        TreeNode child = GetChild(key);
        if (child == null)
        {
            child = AddChild(key);
        }
        return child;
    }

    /// <summary>
    /// ��ȡ�ӽڵ�
    /// </summary>
    public TreeNode GetChild(RangeString key)
    {

        if (children == null)
        {
            return null;
        }

        children.TryGetValue(key, out TreeNode child);
        return child;
    }

    /// <summary>
    /// ����ӽڵ�
    /// </summary>
    public TreeNode AddChild(RangeString key)
    {
        if (children == null)
        {
            children = new Dictionary<RangeString, TreeNode>();
        }
        else if (children.ContainsKey(key))
        {
            throw new Exception("�ӽڵ����ʧ�ܣ��������ظ���ӣ�" + FullPath);
        }

        TreeNode child = new TreeNode(key.ToString(), this);
        children.Add(key, child);
        RedPointMgr.Instance.NodeNumChangeCallback?.Invoke();
        return child;
    }

    /// <summary>
    /// �Ƴ��ӽڵ�
    /// </summary>
    public bool RemoveChild(RangeString key)
    {
        if (children == null || children.Count == 0)
        {
            return false;
        }

        TreeNode child = GetChild(key);

        if (child != null)
        {
            //�ӽڵ㱻ɾ�� ��Ҫ����һ�θ��ڵ�ˢ��
            RedPointMgr.Instance.MarkDirtyNode(this);

            children.Remove(key);

            RedPointMgr.Instance.NodeNumChangeCallback?.Invoke();

            return true;
        }

        return false;
    }

    /// <summary>
    /// �Ƴ������ӽڵ�
    /// </summary>
    public void RemoveAllChild()
    {
        if (children == null || children.Count == 0)
        {
            return;
        }

        children.Clear();
        RedPointMgr.Instance.MarkDirtyNode(this);
        RedPointMgr.Instance.NodeNumChangeCallback?.Invoke();
    }

    public override string ToString()
    {
        return FullPath;
    }

    /// <summary>
    /// �ı�ڵ�ֵ
    /// </summary>
    private void InternalChangeValue(int newValue)
    {
        if (Value == newValue)
        {
            return;
        }

        Value = newValue;
        changeCallback?.Invoke(newValue);
        RedPointMgr.Instance.NodeValueChangeCallback?.Invoke(this, Value);

        //��Ǹ��ڵ�Ϊ��ڵ�
        RedPointMgr.Instance.MarkDirtyNode(Parent);
    }
}
