using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YSH.Framework
{
    public class ResourcesMgr : Singleton<ResourcesMgr>
    {
        private Dictionary<string, UnityEngine.Object> resourceCacheDic = new Dictionary<string, UnityEngine.Object>();

        /// <summary>
        /// 同步加载资源
        /// </summary>
        public T Load<T>(string path) where T : UnityEngine.Object
        {
            if (resourceCacheDic.TryGetValue(path, out UnityEngine.Object cachedObj))
            {
                return cachedObj as T;
            }

            T obj = Resources.Load<T>(path);
            if (obj != null)
            {
                resourceCacheDic[path] = obj;
            }
            else
            {
                LogMgr.Instance.LogWarning($"[ResourcesMgr] Failed to load resource at path: {path}");
            }
            return obj;
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        public void LoadAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
        {
            if (resourceCacheDic.TryGetValue(path, out UnityEngine.Object cachedObj))
            {
                callback?.Invoke(cachedObj as T);
                return;
            }

            MonoMgr.Instance.StartCoroutine(LoadResourceAsyncCoroutine<T>(path, callback));
        }

        private IEnumerator LoadResourceAsyncCoroutine<T>(string path, Action<T> callback) where T : UnityEngine.Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(path);
            yield return request;

            if (request.asset != null)
            {
                resourceCacheDic[path] = request.asset;
                callback?.Invoke(request.asset as T);
            }
            else
            {
                LogMgr.Instance.LogWarning($"[ResourcesMgr] Failed to async load resource at path: {path}");
                callback?.Invoke(null);
            }
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        public void Unload(string path)
        {
            if (resourceCacheDic.TryGetValue(path, out UnityEngine.Object obj))
            {
                Resources.UnloadAsset(obj);
                resourceCacheDic.Remove(path);
            }
        }

        /// <summary>
        /// 清空缓存（不卸载实例化后的资源）
        /// </summary>
        public void ClearCache()
        {
            resourceCacheDic.Clear();
        }
    }
}
