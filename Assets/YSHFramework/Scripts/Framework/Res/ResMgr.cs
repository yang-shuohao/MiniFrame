using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace YSH.Framework
{
    public enum ResLoadType
    {
        Resources,
        Addressables,
    }

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

    public class ResMgr : Singleton<ResMgr>
    {
        //Ĭ����Դ���ط�ʽ
        public ResLoadType resLoadType = ResLoadType.Addressables;

        #region Addressable����
        //�������м��ص��ʲ�
        public Dictionary<string, AddressablesInfo> assetDic = new Dictionary<string, AddressablesInfo>();

        //�첽������Դ�ķ���
        private void LoadAAAsync<T>(string assetName, Action<T> callBack)
        {
            string keyName = assetName + "_" + typeof(T).Name;

            // �ȳ��Ի�ȡ�Ѽ��ص���Դ
            if (assetDic.TryGetValue(keyName, out AddressablesInfo info))
            {
                // ���ü��� +1
                info.count++;

                var handle = info.handle.Convert<T>();

                // ��Դ�Ѽ�����ɣ�ֱ�ӻص�
                if (handle.IsDone)
                {
                    callBack?.Invoke(handle.Result);
                }
                else
                {
                    handle.Completed += obj => OnLoadCompleted(obj, keyName, callBack);
                }
            }
            else
            {
                // ��Դδ���أ���ʼ�첽����
                var handle = Addressables.LoadAssetAsync<T>(assetName);
                handle.Completed += obj => OnLoadCompleted(obj, keyName, callBack);

                // ��¼���ֵ�
                assetDic[keyName] = new AddressablesInfo(handle);
            }
        }

        // ͳһ�ļ�����ɴ���
        private void OnLoadCompleted<T>(AsyncOperationHandle<T> handle, string keyName, Action<T> callBack)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                callBack?.Invoke(handle.Result);
            }
            else
            {
                assetDic.Remove(keyName);
                LogMgr.Instance.LogWarning($"{keyName} ��Դ����ʧ��");
            }
        }

        //�ͷ���Դ�ķ��� 
        public void Release<T>(string name)
        {
            string keyName = name + "_" + typeof(T).Name;

            // ���Ի�ȡ��Դ
            if (assetDic.TryGetValue(keyName, out AddressablesInfo info))
            {
                // �ͷ�ʱ ���ü���-1
                info.count--;

                // ������ü���Ϊ0 ���������ͷ�
                if (info.count == 0)
                {
                    // ȷ����Դ�����Ч
                    if (info.handle.IsValid())
                    {
                        AsyncOperationHandle<T> handle = info.handle.Convert<T>();
                        Addressables.Release(handle);

                        // ��Դ�ͷź��Ƴ��ֵ�
                        assetDic.Remove(keyName);
                    }
                    else
                    {
                        LogMgr.Instance.LogWarning($"{keyName} ����Դ�����Ч���޷��ͷţ�");
                    }
                }
            }
            else
            {
                LogMgr.Instance.LogWarning($"�޷��ҵ���Դ {keyName}���ͷ�ʧ�ܣ�");
            }
        }
        #endregion

        #region Resources����
        // ͨ�õ��첽���ط���
        private void LoadAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object
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

        // ͬ�����ط���
        public T Load<T>(string path) where T : UnityEngine.Object
        {
            // ͬ��������Դ
            T loadedResource = Resources.Load<T>(path);
            return loadedResource;
        }
        #endregion

        public void LoadAssetAsync<T>(string assetName, ResLoadType resLoadType, Action<T> callBack) where T : UnityEngine.Object
        {
            switch (resLoadType)
            {
                case ResLoadType.Resources:
                    LoadAsync<T>(assetName, callBack);
                    break;
                case ResLoadType.Addressables:
                    LoadAAAsync<T>(assetName, callBack);
                    break;
            }
        }

        public void LoadAssetAsync<T>(string assetName, Action<T> callBack) where T : UnityEngine.Object
        {
            LoadAssetAsync(assetName, resLoadType, callBack);
        }

        // �ͷ�������Դ
        public void ReleaseAll()
        {
            foreach (var kvp in assetDic)
            {
                if (kvp.Value.handle.IsValid())
                {
                    Addressables.Release(kvp.Value.handle);
                }
            }
            assetDic.Clear();
        }
    }
}
