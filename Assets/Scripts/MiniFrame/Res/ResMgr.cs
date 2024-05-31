
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesInfo
{
    //�첽�������
    public AsyncOperationHandle handle;
    //���ü���
    public uint count;

    public AddressablesInfo(AsyncOperationHandle handle)
    {
        this.handle = handle;
        count += 1;
    }
}

/// <summary>
/// ��Դ�����࣬����ͳһ������Դ��Addressable��
/// </summary>
public class ResMgr : Singleton<ResMgr>
{
    #region Addressable
    //�������м��ص��ʲ�
    public Dictionary<string, AddressablesInfo> assetDic = new Dictionary<string, AddressablesInfo>();

    //�첽������Դ�ķ���
    public void LoadAssetAsync<T>(string assetName, Action<AsyncOperationHandle<T>> callBack)
    {
        //���ڴ���ͬ�� ��ͬ������Դ�����ּ���
        //��������ͨ�����ֺ�����ƴ����Ϊ key
        string keyName = assetName + "_" + typeof(T).Name;

        AsyncOperationHandle<T> handle;
        //����Ѿ����ع�����Դ
        if (assetDic.ContainsKey(keyName))
        {
            //��ȡ�첽���ط��صĲ�������
            handle = assetDic[keyName].handle.Convert<T>();
            //Ҫʹ����Դ�� ��ô���ü���+1
            assetDic[keyName].count += 1;
            //�ж� ����첽�����Ƿ����
            if (handle.IsDone)
            {
                callBack(handle);
            }
            else
            {
                handle.Completed += (obj) =>
                {
                    if (obj.Status == AsyncOperationStatus.Succeeded)
                    {
                        callBack(obj);
                    }
                };
            }
        }
        else
        {
            //���û�м��ع�����Դ
            //ֱ�ӽ����첽���� ���Ҽ�¼
            handle = Addressables.LoadAssetAsync<T>(assetName);
            handle.Completed += (obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    callBack(obj);
                }
                else
                {
                    if (assetDic.ContainsKey(keyName))
                    {
                        assetDic.Remove(keyName);
                    }
                    Debug.LogWarning(keyName + "��Դ����ʧ��");
                }
            };

            AddressablesInfo info = new AddressablesInfo(handle);
            assetDic.Add(keyName, info);
        }
    }

    //�ͷ���Դ�ķ��� 
    public void Release<T>(string name)
    {
        //���ڴ���ͬ�� ��ͬ������Դ�����ּ���
        //��������ͨ�����ֺ�����ƴ����Ϊ key
        string keyName = name + "_" + typeof(T).Name;

        if (assetDic.ContainsKey(keyName))
        {
            //�ͷ�ʱ ���ü���-1
            assetDic[keyName].count -= 1;

            //������ü���Ϊ0  ���������ͷ�
            if (assetDic[keyName].count == 0)
            {
                //ȡ������ �Ƴ���Դ ���Ҵ��ֵ������Ƴ�
                if (assetDic[keyName].handle.IsValid())
                {
                    AsyncOperationHandle<T> handle = assetDic[keyName].handle.Convert<T>();
                    Addressables.Release(handle);
                    assetDic.Remove(keyName);
                }
            }
        }
    }
    #endregion

    #region Resources
    // ͨ�õ��첽���ط���
    public void LoadAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object
    {
        MonoMgr.Instance.StartCoroutine(LoadResourceCoroutine(path, callBack));
    }

    // Э�̷����������첽������Դ
    private IEnumerator LoadResourceCoroutine<T>(string path, Action<T> callBack) where T : UnityEngine.Object
    {
        // ��ʼ�첽������Դ
        ResourceRequest resourceRequest = Resources.LoadAsync<T>(path);

        // �ȴ��������
        yield return resourceRequest;

        // ������ɺ��ȡ��Դ
        T loadedResource = resourceRequest.asset as T;

        // ���ûص�����
        if (callBack != null)
        {
            callBack(loadedResource);
        }
    }

    // ͨ�õ�ͬ�����ط���
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        // ͬ��������Դ
        T loadedResource = Resources.Load<T>(path);
        return loadedResource;
    }
    #endregion
}
