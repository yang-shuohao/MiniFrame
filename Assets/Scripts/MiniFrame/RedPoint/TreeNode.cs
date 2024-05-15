using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    /// <summary>
    /// 子节点
    /// </summary>
    private Dictionary<RangeString, TreeNode> children;

    /// <summary>
    /// 节点值改变回调
    /// </summary>
    private Action<int> changeCallback;

    /// <summary>
    /// 完整路径
    /// </summary>
    private string fullPath;

    /// <summary>
    /// 节点名
    /// </summary>
    public string Name
    {
        get;
        private set;
    }

    /// <summary>
    /// 完整路径
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
    /// 节点值
    /// </summary>
    public int Value
    {
        get;
        private set;
    }

    /// <summary>
    /// 父节点
    /// </summary>
    public TreeNode Parent
    {
        get;
        private set;
    }

    /// <summary>
    /// 子节点
    /// </summary>
    public Dictionary<RangeString, TreeNode>.ValueCollection Children
    {
        get
        {
            return children?.Values;
        }
    }

    /// <summary>
    /// 子节点数量
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
    /// 添加节点值监听
    /// </summary>
    public void AddListener(Action<int> callback)
    {
        changeCallback += callback;
    }

    /// <summary>
    /// 移除节点值监听
    /// </summary>
    public void RemoveListener(Action<int> callback)
    {
        changeCallback -= callback;
    }

    /// <summary>
    /// 移除所有节点值监听
    /// </summary>
    public void RemoveAllListener()
    {
        changeCallback = null;
    }

    /// <summary>
    /// 改变节点值（使用传入的新值，只能在叶子节点上调用）
    /// </summary>
    public void ChangeValue(int newValue)
    {
        if (children != null && children.Count != 0)
        {
            throw new Exception("不允许直接改变非叶子节点的值：" + FullPath);
        }

        InternalChangeValue(newValue);
    }

    /// <summary>
    /// 改变节点值（根据子节点值计算新值，只对非叶子节点有效）
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
    /// 获取子节点，如果不存在则添加
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
    /// 获取子节点
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
    /// 添加子节点
    /// </summary>
    public TreeNode AddChild(RangeString key)
    {
        if (children == null)
        {
            children = new Dictionary<RangeString, TreeNode>();
        }
        else if (children.ContainsKey(key))
        {
            throw new Exception("子节点添加失败，不允许重复添加：" + FullPath);
        }

        TreeNode child = new TreeNode(key.ToString(), this);
        children.Add(key, child);
        RedPointMgr.Instance.NodeNumChangeCallback?.Invoke();
        return child;
    }

    /// <summary>
    /// 移除子节点
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
            //子节点被删除 需要进行一次父节点刷新
            RedPointMgr.Instance.MarkDirtyNode(this);

            children.Remove(key);

            RedPointMgr.Instance.NodeNumChangeCallback?.Invoke();

            return true;
        }

        return false;
    }

    /// <summary>
    /// 移除所有子节点
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
    /// 改变节点值
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

        //标记父节点为脏节点
        RedPointMgr.Instance.MarkDirtyNode(Parent);
    }
}
