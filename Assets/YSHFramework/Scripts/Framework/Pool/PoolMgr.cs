
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YSH.Framework
{
    public class PoolMgr : Singleton<PoolMgr>
    {
        //���������������ĸ�����
        private GameObject poolParent;
        //�������
        private Dictionary<string, PoolInfo> poolDic = new Dictionary<string, PoolInfo>();
        //�������
        private Dictionary<GameObject, Component> comDic = new Dictionary<GameObject, Component>();

        #region �Ӷ���ػ�ȡ����
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
                //ֱ��ȡ
                callBack?.Invoke(poolDic[name].Get(parent, worldPositionStays));
            }
            else
            {
                //����
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
                //ֱ��ȡ
                callBack?.Invoke(poolDic[name].Get(position, rotation, parent));
            }
            else
            {
                //����
                ResMgr.Instance.LoadAssetAsync<GameObject>(name, ResMgr.Instance.resLoadType, result =>
                {
                    GameObject go = GameObject.Instantiate(result, position, rotation, parent);
                    go.name = name;

                    callBack?.Invoke(go);
                });
            }
        }
        #endregion

        #region �Ӷ�����л�ȡ�����ϵ����

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
        /// ����Żض����
        /// </summary>
        public void Release(GameObject obj)
        {
            if (poolParent == null)
            {
                poolParent = new GameObject("PoolParent");
            }

            // ��ֹ�ظ��黹
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
        /// ��ն���أ������л���
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
            //������
            public GameObject parentObject;
            //���󼯺�
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

                //ע��UI���������
                Vector3 scale = obj.transform.localScale;
                obj.transform.SetParent(parentObject.transform);
                obj.transform.localScale = scale;

                objects.Add(obj);
            }
        }
    }
}

