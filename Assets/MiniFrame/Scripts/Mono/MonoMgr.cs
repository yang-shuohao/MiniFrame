
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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
    /// ���֡���¼���
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        monoController.AddUpdateListener(fun);
    }

    /// <summary>
    /// �Ƴ�֡���¼���
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        monoController.RemoveUpdateListener(fun);
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
