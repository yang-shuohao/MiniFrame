

using UnityEngine.Events;
/// <summary>
/// Mono控制器，提供Update和协程功能
/// </summary>
public class MonoController : MonoSingleton<MonoController>
{
    private event UnityAction updateEvent;

    private void Update()
    {
        updateEvent?.Invoke();
    }

    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }

    public void RemoveUpdateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }
}
