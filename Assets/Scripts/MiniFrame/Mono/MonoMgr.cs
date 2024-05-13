
using System.Collections;
using UnityEngine;

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
