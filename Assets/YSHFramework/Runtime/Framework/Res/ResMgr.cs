using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#if USE_ADDRESSABLES
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif

namespace YSH.Framework
{
    public enum ResLoadType
    {
        Resources,
        Addressables,
    }

#if USE_ADDRESSABLES
    public class AddressablesInfo
    {
        public AsyncOperationHandle handle;
        public uint count;
        public Type assetType;

        public AddressablesInfo(AsyncOperationHandle handle, Type assetType)
        {
            this.handle = handle;
            this.assetType = assetType;
            count = 1;
        }
    }
#endif

    public class ResMgr : Singleton<ResMgr>
    {
        public ResLoadType resLoadType = ResLoadType.Resources;

#if USE_ADDRESSABLES
        private Dictionary<string, AddressablesInfo> assetDic = new Dictionary<string, AddressablesInfo>();

        private StringBuilder sb = new StringBuilder(64);

        private string GetKey(string assetName, string assetTypeName)
        {
            sb.Clear();
            sb.Append(assetName).Append("_").Append(assetTypeName);

            return sb.ToString();
        }

        private void LoadAAAsync<T>(string assetName, Action<T> callBack) where T : UnityEngine.Object
        {
            string key = GetKey(assetName, typeof(T).Name);

            if (assetDic.TryGetValue(key, out AddressablesInfo info))
            {
                info.count++;
                var handle = info.handle.Convert<T>();

                if (handle.IsDone)
                {
                    callBack?.Invoke(handle.Result);
                }
                else
                {
                    handle.Completed += op => OnLoadCompleted(op, key, callBack);
                }
            }
            else
            {
                var handle = Addressables.LoadAssetAsync<T>(assetName);
                handle.Completed += op => OnLoadCompleted(op, key, callBack);
                assetDic[key] = new AddressablesInfo(handle, typeof(T));
            }
        }

        private void OnLoadCompleted<T>(AsyncOperationHandle<T> handle, string key, Action<T> callBack)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                callBack?.Invoke(handle.Result);
            }
            else
            {
                assetDic.Remove(key);
                LogMgr.Instance.LogWarning($"[Addressables] Load Failed: {key}");
            }
        }

        public void Release(string assetName, Type assetType)
        {
            string key = GetKey(assetName, assetType.Name);

            if (assetDic.TryGetValue(key, out AddressablesInfo info))
            {
                info.count--;
                if (info.count <= 0)
                {
                    if (info.handle.IsValid())
                    {
                        Addressables.Release(info.handle);
                        assetDic.Remove(key);
                    }
                    else
                    {
                        LogMgr.Instance.LogWarning($"[Addressables] Invalid handle for release: {key}");
                    }
                }
            }
            else
            {
                LogMgr.Instance.LogWarning($"[Addressables] Release failed, key not found: {key}");
            }
        }

        public void Release<T>(string assetName) where T : UnityEngine.Object
        {
            Release(assetName, typeof(T));
        }

        public void ReleaseAll()
        {
            foreach (var kv in assetDic)
            {
                if (kv.Value.handle.IsValid())
                {
                    Addressables.Release(kv.Value.handle);
                }
            }
            assetDic.Clear();
        }

#endif
        #region Resourcesº”‘ÿ

        private void LoadAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object
        {
            MonoMgr.Instance.StartCoroutine(LoadResourceCoroutine(path, callBack));
        }

        private IEnumerator LoadResourceCoroutine<T>(string path, Action<T> callBack) where T : UnityEngine.Object
        {
            ResourceRequest req = Resources.LoadAsync<T>(path);
            yield return req;

            if (req.asset is T result)
            {
                callBack?.Invoke(result);
            }
            else
            {
                LogMgr.Instance.LogWarning($"[Resources] Load failed: {path}");
            }
        }

        public T Load<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        #endregion

        public void LoadAssetAsync<T>(string assetName, ResLoadType type, Action<T> callBack) where T : UnityEngine.Object
        {
            switch (type)
            {
                case ResLoadType.Resources:
                    LoadAsync(assetName, callBack);
                    break;
#if USE_ADDRESSABLES
                case ResLoadType.Addressables:
                    LoadAAAsync(assetName, callBack);
                    break;
#endif
            }
        }

        public void LoadAssetAsync<T>(string assetName, Action<T> callBack) where T : UnityEngine.Object
        {
            LoadAssetAsync(assetName, resLoadType, callBack);
        }
    }
}
