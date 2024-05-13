
using System.Collections;
using UnityEngine;

/// <summary>
/// Mono������
/// </summary>
public class MonoMgr : Singleton<MonoMgr>
{
    private MonoController monoController;

    /// <summary>
    /// ����MonoController
    /// </summary>
    public MonoMgr()
    {
        GameObject go = new GameObject("MonoController");
        monoController = go.AddComponent<MonoController>();
    }

    /// <summary>
    /// ����Э��
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return monoController.StartCoroutine(routine);
    }

    /// <summary>
    /// ֹͣ����Э��
    /// </summary>
    public void StopAllCoroutines()
    {
        monoController.StopAllCoroutines();
    }

    /// <summary>
    /// ֹͣЭ��
    /// </summary>
    /// <param name="routine"></param>
    public void StopCoroutine(IEnumerator routine)
    {
        monoController.StopCoroutine(routine);
    }

    /// <summary>
    /// ֹͣЭ��
    /// </summary>
    /// <param name="routine"></param>
    public void StopCoroutine(Coroutine routine)
    {
        monoController.StopCoroutine(routine);
    }

}
