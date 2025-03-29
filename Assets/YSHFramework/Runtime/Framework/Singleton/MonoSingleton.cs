
using UnityEngine;

namespace YSH.Framework
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected MonoSingleton() { }

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
                    }
                }

                return instance;
            }
        }

    }
}
