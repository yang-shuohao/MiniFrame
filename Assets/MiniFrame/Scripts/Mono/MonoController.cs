

using UnityEngine.Events;
/// <summary>
/// Mono���������ṩUpdate��Э�̹���
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
