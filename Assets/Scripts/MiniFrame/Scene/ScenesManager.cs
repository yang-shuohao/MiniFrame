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
    /// 场景管理器
    /// </summary>
    public class ScenesManager : Singleton<ScenesManager>
    {
        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void LoadScene(string name, UnityAction action)
        {
            SceneManager.LoadScene(name);
            action?.Invoke();
        }

        /// <summary>
        /// 异步加载场景
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
                //更新进度条
                EventManager.Instance.EventDispatcher<float>("LoadSceneProgressEvent", ao.progress);

                yield return null;
            }

            acion?.Invoke();
        }
    }
}

