
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// 对象池信息
/// </summary>
public class PoolInfo
{
    //父物体
    public GameObject parentObject;
    //对象集合
    public List<GameObject> objects;

    public PoolInfo(GameObject obj, GameObject poolParent)
    {
        parentObject = new GameObject(obj.name);
        parentObject.transform.parent = poolParent.transform;

        objects = new List<GameObject>();

        ReleaseToPool(obj);
    }

    public GameObject GetFromPool()
    {
        GameObject obj = null;
        //获取
        obj = objects[0];
        //移除
        objects.RemoveAt(0);
        //激活
        obj.SetActive(true);
        //设置父物体
        obj.transform.parent = null;

        return obj;
    }

    public void ReleaseToPool(GameObject obj)
    {
        //失活
        obj.SetActive(false);
        //添加
        objects.Add(obj);
        //设置父物体
        obj.transform.parent = parentObject.transform;
    }
}


/// <summary>
/// 池管理器
/// </summary>
public class PoolMgr : Singleton<PoolMgr>
{
    //对象池中所有物体的父物体
    private GameObject poolParent;
    //根据名字保存对象
    private Dictionary<string, PoolInfo> poolDic = new Dictionary<string, PoolInfo>();

    /// <summary>
    /// 从对象池中获取对象
    /// </summary>
    /// <param name="name">名字</param>
    /// <param name="callBack">获取后的回调</param>
    public void GetFromPool(string name, UnityAction<GameObject> callBack)
    {
        if (poolDic.ContainsKey(name) && poolDic[name].objects.Count > 0)
        {
            //直接取
            callBack?.Invoke(poolDic[name].GetFromPool());
        }
        else
        {
            //创建
            //加载
            ResMgr.Instance.LoadAssetAsync<GameObject>(name, (handle) =>
            {
                GameObject go = GameObject.Instantiate(handle.Result);
                go.name = name;

                callBack?.Invoke(go);
            });
        }
    }

    /// <summary>
    /// 从对象池中获取对象上的组件
    /// </summary>
    /// <param name="name">名字</param>
    /// <param name="callBack">获取后的回调</param>
    public void GetFromPool<T>(string name, UnityAction<T> callBack) where T : Component
    {
        if (poolDic.ContainsKey(name) && poolDic[name].objects.Count > 0)
        {
            //直接取
            callBack?.Invoke(poolDic[name].GetFromPool().GetComponent<T>());
        }
        else
        {
            //直接创建物体添加组件
            GameObject go = new GameObject(name);
            callBack?.Invoke(go.AddComponent<T>());
        }
    }

    /// <summary>
    /// 物体放回对象池
    /// </summary>
    /// <param name="obj">被放回的对象池</param>
    public void ReleaseToPool(GameObject obj)
    {
        if (poolParent == null)
        {
            poolParent = new GameObject("PoolParent");
        }

        if (poolDic.ContainsKey(obj.name))
        {
            poolDic[obj.name].ReleaseToPool(obj);
        }
        else
        {
            poolDic.Add(obj.name, new PoolInfo(obj, poolParent));
        }
    }

    /// <summary>
    /// 清空对象池（场景切换）
    /// </summary>
    public void ClearPool()
    {
        poolDic.Clear();
        poolDic = null;
    }
}
