
using UnityEngine;

namespace YSH.Framework
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly object threadLock = new object();

        private static bool isQuitting = false;

        private static T instance;
        public static T Instance
        {
            get
            {
                if(isQuitting)
                {
                    return null;
                }

                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    lock(threadLock)
                    {
                        if (instance == null)
                        {
                            GameObject go = new GameObject(typeof(T).Name);
                            instance = go.AddComponent<T>();
                            DontDestroyOnLoad(go);
                        }
                    }
                }

                return instance;
            }
        }

        protected virtual void OnDestroy()
        {
            isQuitting = true;
        }
    }
}
