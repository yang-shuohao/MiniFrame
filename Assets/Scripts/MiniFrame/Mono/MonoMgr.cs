
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mono管理器
/// </summary>
public class MonoMgr : Singleton<MonoMgr>
{
    private MonoController monoController;

    /// <summary>
    /// 生成MonoController
    /// </summary>
    public MonoMgr()
    {
        GameObject go = new GameObject("MonoController");
        monoController = go.AddComponent<MonoController>();
    }

    /// <summary>
    /// 添加帧更新监听
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        monoController.AddUpdateListener(fun);
    }

    /// <summary>
    /// 移除帧更新监听
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        monoController.RemoveUpdateListener(fun);
    }

    /// <summary>
    /// 开启协程
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return monoController.StartCoroutine(routine);
    }

    /// <summary>
    /// 停止所有协程
    /// </summary>
    public void StopAllCoroutines()
    {
        monoController.StopAllCoroutines();
    }

    /// <summary>
    /// 停止协程
    /// </summary>
    /// <param name="routine"></param>
    public void StopCoroutine(IEnumerator routine)
    {
        monoController.StopCoroutine(routine);
    }

    /// <summary>
    /// 停止协程
    /// </summary>
    /// <param name="routine"></param>
    public void StopCoroutine(Coroutine routine)
    {
        monoController.StopCoroutine(routine);
    }

}
