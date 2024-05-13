

/// <summary>
/// Mono控制器，提供生命周期函数和协程功能
/// </summary>
public class MonoController : MonoSingleton<MonoController>
{
    /// <summary>
    /// 给不继承MonoBehaviour的类派发Update事件
    /// </summary>
    private void Update()
    {
        EventMgr.Instance.EventDispatcher(MonoEventName.Update);
    }
}
