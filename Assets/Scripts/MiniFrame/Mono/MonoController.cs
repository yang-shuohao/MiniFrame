

/// <summary>
/// Mono���������ṩ�������ں�����Э�̹���
/// </summary>
public class MonoController : MonoSingleton<MonoController>
{
    /// <summary>
    /// �����̳�MonoBehaviour�����ɷ�Update�¼�
    /// </summary>
    private void Update()
    {
        EventMgr.Instance.EventDispatcher(MonoEventName.Update);
    }
}
