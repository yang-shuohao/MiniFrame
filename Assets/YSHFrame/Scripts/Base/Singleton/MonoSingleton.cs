
using UnityEngine;

namespace YSH.Framework
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected MonoSingleton() { }

        //退出编辑器播放模式时，在OnDestroy里面调用会出现错误，使用此进行判断
        public static bool IsExisted { get; private set; } = false;

        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        GameObject go = new GameObject(typeof(T).Name);
                        instance = go.AddComponent<T>();
                        DontDestroyOnLoad(go);

                        IsExisted = true;
                    }
                }

                return instance;
            }
        }

        protected virtual void OnDestroy()
        {
            IsExisted = false;
        }
    }
}
