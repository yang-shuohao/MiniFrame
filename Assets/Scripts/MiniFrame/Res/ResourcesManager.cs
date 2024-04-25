
using MiniFrame.Base;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MiniFrame.Res
{
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
    public class ResourcesManager : Singleton<ResourcesManager>
    {
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
    }
}


