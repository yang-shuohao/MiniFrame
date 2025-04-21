
namespace YSH.Framework
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        protected Singleton() { }

        private static readonly object threadLock = new object();

        private static volatile T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (threadLock)
                    {
                        if (instance == null)
                        {
                            instance = new T();
                        }
                    }
                }
                return instance;
            }
        }

    }
}
