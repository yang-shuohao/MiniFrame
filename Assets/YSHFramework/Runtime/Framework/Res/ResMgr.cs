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
        //异步操作句柄
        public AsyncOperationHandle handle;
        //引用计数
        public uint count;

        public AddressablesInfo(AsyncOperationHandle handle)
        {
            this.handle = handle;
            count += 1;
        }
    }

    public class ResMgr : Singleton<ResMgr>
    {
        //默认资源加载方式
        public ResLoadType resLoadType = ResLoadType.Addressables;

        #region Addressable加载
        //保存所有加载的资产
        public Dictionary<string, AddressablesInfo> assetDic = new Dictionary<string, AddressablesInfo>();

        //异步加载资源的方法
        private void LoadAAAsync<T>(string assetName, Action<T> callBack)
        {
            string keyName = assetName + "_" + typeof(T).Name;

            // 先尝试获取已加载的资源
            if (assetDic.TryGetValue(keyName, out AddressablesInfo info))
            {
                // 引用计数 +1
                info.count++;

                var handle = info.handle.Convert<T>();

                // 资源已加载完成，直接回调
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
                // 资源未加载，开始异步加载
                var handle = Addressables.LoadAssetAsync<T>(assetName);
                handle.Completed += obj => OnLoadCompleted(obj, keyName, callBack);

                // 记录到字典
                assetDic[keyName] = new AddressablesInfo(handle);
            }
        }

        // 统一的加载完成处理
        private void OnLoadCompleted<T>(AsyncOperationHandle<T> handle, string keyName, Action<T> callBack)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                callBack?.Invoke(handle.Result);
            }
            else
            {
                assetDic.Remove(keyName);
                LogMgr.Instance.LogWarning($"{keyName} 资源加载失败");
            }
        }

        //释放资源的方法 
        public void Release<T>(string name)
        {
            string keyName = name + "_" + typeof(T).Name;

            // 尝试获取资源
            if (assetDic.TryGetValue(keyName, out AddressablesInfo info))
            {
                // 释放时 引用计数-1
                info.count--;

                // 如果引用计数为0 才真正的释放
                if (info.count == 0)
                {
                    // 确保资源句柄有效
                    if (info.handle.IsValid())
                    {
                        AsyncOperationHandle<T> handle = info.handle.Convert<T>();
                        Addressables.Release(handle);

                        // 资源释放后移除字典
                        assetDic.Remove(keyName);
                    }
                    else
                    {
                        LogMgr.Instance.LogWarning($"{keyName} 的资源句柄无效，无法释放！");
                    }
                }
            }
            else
            {
                LogMgr.Instance.LogWarning($"无法找到资源 {keyName}，释放失败！");
            }
        }
        #endregion

        #region Resources加载
        // 通用的异步加载方法
        private void LoadAsync<T>(string path, Action<T> callBack) where T : UnityEngine.Object
        {
            MonoMgr.Instance.StartCoroutine(LoadResourceCoroutine(path, callBack));
        }

        // 协程方法，用于异步加载资源
        private IEnumerator LoadResourceCoroutine<T>(string path, Action<T> callBack) where T : UnityEngine.Object
        {
            // 开始异步加载资源
            ResourceRequest resourceRequest = Resources.LoadAsync<T>(path);

            // 等待加载完成
            yield return resourceRequest;

            // 加载完成后获取资源
            T loadedResource = resourceRequest.asset as T;

            // 调用回调函数
            if (callBack != null)
            {
                callBack(loadedResource);
            }
        }

        // 同步加载方法
        public T Load<T>(string path) where T : UnityEngine.Object
        {
            // 同步加载资源
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

        // 释放所有资源
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
