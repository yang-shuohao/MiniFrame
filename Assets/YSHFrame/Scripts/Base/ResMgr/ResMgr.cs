
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace YSH.Framework
{

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

    /// <summary>
    /// 资源管理类，负责统一加载资源（Addressable）
    /// </summary>
    public class ResMgr : Singleton<ResMgr>
    {
        //默认框架资源加载方式
        public ResLoadType resLoadType;

        public ResMgr()
        {
            resLoadType = ResLoadType.Addressables;
        }

        #region Addressable
        //保存所有加载的资产
        public Dictionary<string, AddressablesInfo> assetDic = new Dictionary<string, AddressablesInfo>();

        //异步加载资源的方法
        private void LoadAssetAsync<T>(string assetName, Action<T> callBack)
        {
            //由于存在同名 不同类型资源的区分加载
            //所以我们通过名字和类型拼接作为 key
            string keyName = assetName + "_" + typeof(T).Name;

            AsyncOperationHandle<T> handle;
            //如果已经加载过该资源
            if (assetDic.ContainsKey(keyName))
            {
                //获取异步加载返回的操作内容
                handle = assetDic[keyName].handle.Convert<T>();
                //要使用资源了 那么引用计数+1
                assetDic[keyName].count += 1;
                //判断 这个异步加载是否结束
                if (handle.IsDone)
                {
                    callBack(handle.Result);
                }
                else
                {
                    handle.Completed += (obj) =>
                    {
                        if (obj.Status == AsyncOperationStatus.Succeeded)
                        {
                            callBack(obj.Result);
                        }
                    };
                }
            }
            else
            {
                //如果没有加载过该资源
                //直接进行异步加载 并且记录
                handle = Addressables.LoadAssetAsync<T>(assetName);
                handle.Completed += (obj) =>
                {
                    if (obj.Status == AsyncOperationStatus.Succeeded)
                    {
                        callBack(obj.Result);
                    }
                    else
                    {
                        if (assetDic.ContainsKey(keyName))
                        {
                            assetDic.Remove(keyName);
                        }
                        Debug.LogWarning(keyName + "资源加载失败");
                    }
                };

                AddressablesInfo info = new AddressablesInfo(handle);
                assetDic.Add(keyName, info);
            }
        }

        //释放资源的方法 
        public void Release<T>(string name)
        {
            //由于存在同名 不同类型资源的区分加载
            //所以我们通过名字和类型拼接作为 key
            string keyName = name + "_" + typeof(T).Name;

            if (assetDic.ContainsKey(keyName))
            {
                //释放时 引用计数-1
                assetDic[keyName].count -= 1;

                //如果引用计数为0  才真正的释放
                if (assetDic[keyName].count == 0)
                {
                    //取出对象 移除资源 并且从字典里面移除
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

        // 通用的同步加载方法
        public T Load<T>(string path) where T : UnityEngine.Object
        {
            // 同步加载资源
            T loadedResource = Resources.Load<T>(path);
            return loadedResource;
        }
        #endregion

        /// <summary>
        /// 资源异步加载
        /// </summary>
        /// <typeparam name="T">返回的资源数据</typeparam>
        /// <param name="assetName">资产名/资产路径</param>
        /// <param name="resLoadType">资产加载方式</param>
        /// <param name="callBack">加载完后的回调</param>
        public void LoadAssetAsync<T>(string assetName, ResLoadType resLoadType, Action<T> callBack) where T : UnityEngine.Object
        {
            switch (resLoadType)
            {
                case ResLoadType.Resources:
                    LoadAsync<T>(assetName, callBack);
                    break;
                case ResLoadType.Addressables:
                    LoadAssetAsync<T>(assetName, callBack);
                    break;
            }
        }
    }

}
