
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


/// <summary>
/// ����������
/// </summary>
public class SceneMgr : Singleton<SceneMgr>
{
    /// <summary>
    /// ͬ�����س���
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void LoadScene(string name, UnityAction action)
    {
        SceneManager.LoadScene(name);
        action?.Invoke();
    }

    /// <summary>
    /// �첽���س���
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void LoadSceneAsyn(string name, UnityAction action)
    {
        MonoMgr.Instance.StartCoroutine(LoadSceneAsynImplement(name, action));
    }

    IEnumerator LoadSceneAsynImplement(string name, UnityAction acion)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);

        while (!ao.isDone)
        {
            //���½�����
            EventMgr.Instance.EventDispatcher<float>("LoadSceneProgressEvent", ao.progress);

            yield return null;
        }

        acion?.Invoke();
    }
}

