
using UnityEngine;

/// <summary>
/// �̳�MonoBehaviour�ķǳ־õĵ���������
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
/// �̳�MonoBehaviour�ĳ־õĵ���������
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
/// ���̳�MonoBehaviour�ĵ���������
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