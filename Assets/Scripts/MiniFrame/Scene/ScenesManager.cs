using MiniFrame.Base;
using MiniFrame.Event;
using MiniFrame.Mono;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


namespace MiniFrame.Scene
{
    /// <summary>
    /// ����������
    /// </summary>
    public class ScenesManager : Singleton<ScenesManager>
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
            MonoManager.Instance.StartCoroutine(LoadSceneAsynImplement(name, action));
        }

        IEnumerator LoadSceneAsynImplement(string name, UnityAction acion)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(name);

            while (!ao.isDone)
            {
                //���½�����
                EventManager.Instance.EventDispatcher<float>("LoadSceneProgressEvent", ao.progress);

                yield return null;
            }

            acion?.Invoke();
        }
    }
}

