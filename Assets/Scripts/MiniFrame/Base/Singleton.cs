
using UnityEngine;

/// <summary>
/// 继承MonoBehaviour的非持久的单例抽象类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        Instance = this as T;
    }
}

/// <summary>
/// 继承MonoBehaviour的持久的单例抽象类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MonoSingletonPersistent<T> : MonoSingleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            base.Awake();
        }
    }
}

/// <summary>
/// 不继承MonoBehaviour的单例抽象类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> where T : new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}