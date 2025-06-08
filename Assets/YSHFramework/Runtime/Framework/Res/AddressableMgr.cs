#if USE_ADDRESSABLES

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace YSH.Framework
{
    public class AddressableMgr: Singleton<AddressableMgr>
    {
        class AddressablesInfo
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

        private Dictionary<string, AddressablesInfo> assetDic = new Dictionary<string, AddressablesInfo>();

        public void LoadAssetAsync<T>(string assetName, Action<T> callBack) where T : UnityEngine.Object
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
                LogMgr.Instance.LogWarning($"[AddressableMgr] Load Failed: {key}");
            }
        }

        public void Release<T>(string assetName) where T : UnityEngine.Object
        {
            Release(assetName, typeof(T));
        }

        private void Release(string assetName, Type assetType)
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
                        LogMgr.Instance.LogWarning($"[AddressableMgr] Invalid handle for release: {key}");
                    }
                }
            }
            else
            {
                LogMgr.Instance.LogWarning($"[AddressableMgr] Release failed, key not found: {key}");
            }
        }

        private string GetKey(string assetName, string assetTypeName)
        {
            StringBuilder sb = GenericPool<StringBuilder>.Get();
            sb.Clear();
            sb.Append(assetName).Append("_").Append(assetTypeName);
            string key = sb.ToString();
            GenericPool < StringBuilder >.Release(sb);

            return key;
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
    }
}

#endif
