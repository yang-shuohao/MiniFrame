
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace YSH.Framework
{
    public class SceneMgr : Singleton<SceneMgr>
    {
        public void LoadScene(string name, UnityAction action)
        {
            SceneManager.LoadScene(name);
            action?.Invoke();
        }

        public void LoadSceneAsyn(string name, UnityAction action)
        {
            MonoMgr.Instance.StartCoroutine(LoadSceneAsynImplement(name, action));
        }

        IEnumerator LoadSceneAsynImplement(string name, UnityAction acion)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(name);

            while (!ao.isDone)
            {
                //更新进度条
                EventMgr.Instance.Dispatcher<float>("LoadSceneProgressEvent", ao.progress);

                yield return null;
            }

            acion?.Invoke();
        }
    }

}
