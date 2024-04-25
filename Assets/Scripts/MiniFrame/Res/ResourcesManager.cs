
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
    public class ResourcesManager : Singleton<ResourcesManager>
    {
        //保存所有加载的资产
        public Dictionary<string, AddressablesInfo> assetDic = new Dictionary<string, AddressablesInfo>();

        //异步加载资源的方法
        public void LoadAssetAsync<T>(string assetName, Action<AsyncOperationHandle<T>> callBack)
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
                //如果没有加载过该资源
                //直接进行异步加载 并且记录
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
    }
}


