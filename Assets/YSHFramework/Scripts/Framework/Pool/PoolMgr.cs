
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YSH.Framework
{
    public class PoolMgr : Singleton<PoolMgr>
    {
        //对象池中所有物体的父物体
        private GameObject poolParent;
        //缓存对象
        private Dictionary<string, PoolInfo> poolDic = new Dictionary<string, PoolInfo>();
        //缓存组件
        private Dictionary<GameObject, Component> comDic = new Dictionary<GameObject, Component>();

        #region 从对象池获取物体
        public void Get(string name, Action<GameObject> callBack)
        {
            Get(name, null, callBack);
        }

        public void Get(string name, Transform parent, Action<GameObject> callBack)
        {
            Get(name, parent, true, callBack);
        }

        public void Get(string name, Transform parent, bool worldPositionStays, Action<GameObject> callBack)
        {
            if (poolDic.ContainsKey(name) && poolDic[name].objects.Count > 0)
            {
                //直接取
                callBack?.Invoke(poolDic[name].Get(parent, worldPositionStays));
            }
            else
            {
                //创建
                ResMgr.Instance.LoadAssetAsync<GameObject>(name, ResMgr.Instance.resLoadType, result =>
                {
                    GameObject go = GameObject.Instantiate(result, parent, worldPositionStays);
                    go.name = name;

                    callBack?.Invoke(go);
                });
            }
        }

        public void Get(string name, Vector3 position, Quaternion rotation, Action<GameObject> callBack)
        {
            Get(name, position, rotation, null, callBack);
        }

        public void Get(string name, Vector3 position, Quaternion rotation, Transform parent, Action<GameObject> callBack)
        {
            if (poolDic.ContainsKey(name) && poolDic[name].objects.Count > 0)
            {
                //直接取
                callBack?.Invoke(poolDic[name].Get(position, rotation, parent));
            }
            else
            {
                //创建
                ResMgr.Instance.LoadAssetAsync<GameObject>(name, ResMgr.Instance.resLoadType, result =>
                {
                    GameObject go = GameObject.Instantiate(result, position, rotation, parent);
                    go.name = name;

                    callBack?.Invoke(go);
                });
            }
        }
        #endregion

        #region 从对象池中获取对象上的组件

        public void Get<T>(string name, UnityAction<T> callBack) where T : Component
        {
            Get(name, null, callBack);
        }

        public void Get<T>(string name, Transform parent, UnityAction<T> callBack) where T : Component
        {
            Get(name, parent, true, callBack);
        }

        public void Get<T>(string name, Transform parent, bool worldPositionStays, UnityAction<T> callBack) where T : Component
        {
            Get(name, parent, worldPositionStays, (obj) =>
            {
                T com = null;
                if (!comDic.ContainsKey(obj))
                {
                    com = obj.GetComponent<T>();
                    comDic.Add(obj, com);
                }
                else
                {
                    com = comDic[obj] as T;
                }
                callBack?.Invoke(com);
            });
        }

        public void Get<T>(string name, Vector3 position, Quaternion rotation, UnityAction<T> callBack) where T : Component
        {
            Get(name, position, rotation, null, callBack);
        }

        public void Get<T>(string name, Vector3 position, Quaternion rotation, Transform parent, UnityAction<T> callBack) where T : Component
        {
            Get(name, position, rotation, parent, (obj) =>
            {
                T com = null;
                if (!comDic.ContainsKey(obj))
                {
                    com = obj.GetComponent<T>();
                    comDic.Add(obj, com);
                }
                else
                {
                    com = comDic[obj] as T;
                }
                callBack?.Invoke(com);
            });
        }

        #endregion

        /// <summary>
        /// 物体放回对象池
        /// </summary>
        public void Release(GameObject obj)
        {
            if (poolParent == null)
            {
                poolParent = new GameObject("PoolParent");
            }

            // 防止重复归还
            if (poolDic.ContainsKey(obj.name) && poolDic[obj.name].objects.Contains(obj))
            {
                LogMgr.Instance.LogWarning($"Object {obj.name} is already in the pool.");
                return;
            }

            if (poolDic.ContainsKey(obj.name))
            {
                poolDic[obj.name].Release(obj);
            }
            else
            {
                poolDic.Add(obj.name, new PoolInfo(obj, poolParent));
            }
        }

        /// <summary>
        /// 清空对象池（场景切换）
        /// </summary>
        public void ClearPool()
        {
            poolDic.Clear();
            comDic.Clear();
            poolDic = null;
            comDic = null;
        }

        private class PoolInfo
        {
            //父物体
            public GameObject parentObject;
            //对象集合
            public List<GameObject> objects;

            public PoolInfo(GameObject obj, GameObject poolParent)
            {
                parentObject = new GameObject(obj.name);

                parentObject.transform.parent = poolParent.transform;

                objects = new List<GameObject>();

                Release(obj);
            }

            public GameObject Get()
            {
                return Get(null);
            }

            public GameObject Get(Transform parent)
            {
                return Get(parent, true);
            }

            public GameObject Get(Transform parent, bool worldPositionStays)
            {
                int lastIndex = objects.Count - 1;
                GameObject obj = objects[lastIndex];
                objects.RemoveAt(lastIndex);

                obj.transform.SetParent(parent, worldPositionStays);
                obj.SetActive(true);

                return obj;
            }

            public GameObject Get(Vector3 position, Quaternion rotation)
            {
                return Get(position, rotation, null);
            }

            public GameObject Get(Vector3 position, Quaternion rotation, Transform parent)
            {
                int lastIndex = objects.Count - 1;
                GameObject obj = objects[lastIndex];
                objects.RemoveAt(lastIndex);

                obj.transform.SetParent(parent);
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);

                return obj;
            }

            public void Release(GameObject obj)
            {
                obj.SetActive(false);

                //注意UI物体的缩放
                Vector3 scale = obj.transform.localScale;
                obj.transform.SetParent(parentObject.transform);
                obj.transform.localScale = scale;

                objects.Add(obj);
            }
        }
    }
}

