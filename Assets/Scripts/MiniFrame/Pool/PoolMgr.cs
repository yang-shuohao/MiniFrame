
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// �������Ϣ
/// </summary>
public class PoolInfo
{
    //������
    public GameObject parentObject;
    //���󼯺�
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
        //��ȡ
        obj = objects[0];
        //�Ƴ�
        objects.RemoveAt(0);
        //����
        obj.SetActive(true);
        //���ø�����
        obj.transform.parent = null;

        return obj;
    }

    public void ReleaseToPool(GameObject obj)
    {
        //ʧ��
        obj.SetActive(false);
        //���
        objects.Add(obj);
        //���ø�����
        obj.transform.parent = parentObject.transform;
    }
}


/// <summary>
/// �ع�����
/// </summary>
public class PoolMgr : Singleton<PoolMgr>
{
    //���������������ĸ�����
    private GameObject poolParent;
    //�������ֱ������
    private Dictionary<string, PoolInfo> poolDic = new Dictionary<string, PoolInfo>();

    /// <summary>
    /// �Ӷ�����л�ȡ����
    /// </summary>
    /// <param name="name">����</param>
    /// <param name="callBack">��ȡ��Ļص�</param>
    public void GetFromPool(string name, UnityAction<GameObject> callBack)
    {
        if (poolDic.ContainsKey(name) && poolDic[name].objects.Count > 0)
        {
            //ֱ��ȡ
            callBack?.Invoke(poolDic[name].GetFromPool());
        }
        else
        {
            //����
            //����
            ResMgr.Instance.LoadAssetAsync<GameObject>(name, (handle) =>
            {
                GameObject go = GameObject.Instantiate(handle.Result);
                go.name = name;

                callBack?.Invoke(go);
            });
        }
    }

    /// <summary>
    /// �Ӷ�����л�ȡ�����ϵ����
    /// </summary>
    /// <param name="name">����</param>
    /// <param name="callBack">��ȡ��Ļص�</param>
    public void GetFromPool<T>(string name, UnityAction<T> callBack) where T : Component
    {
        if (poolDic.ContainsKey(name) && poolDic[name].objects.Count > 0)
        {
            //ֱ��ȡ
            callBack?.Invoke(poolDic[name].GetFromPool().GetComponent<T>());
        }
        else
        {
            //ֱ�Ӵ�������������
            GameObject go = new GameObject(name);
            callBack?.Invoke(go.AddComponent<T>());
        }
    }

    /// <summary>
    /// ����Żض����
    /// </summary>
    /// <param name="obj">���ŻصĶ����</param>
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
    /// ��ն���أ������л���
    /// </summary>
    public void ClearPool()
    {
        poolDic.Clear();
        poolDic = null;
    }
}
