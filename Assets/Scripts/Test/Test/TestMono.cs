

using System.Collections;
using UnityEngine;
using YSH.Framework;
using DG.Tweening;
using YSH.Framework.Utils;

public class TestMono : MonoBehaviour
{
    private bool isExit;
    private bool isExit2;

    private void Start()
    {
        StartCoroutine(Coroutine1());
    }

    IEnumerator Coroutine1()
    {
        WaitForSecondsRealtime waitForSecondsRealtime = CoroutineCache.GetWaitForSecondsRealtime(3f);
        while (!isExit)
        {
            Debug.Log("Coroutine1开始" + Time.time);
            yield return waitForSecondsRealtime;
            Debug.Log("Coroutine1结束" + Time.time);
        }

        CoroutineCache.ReleaseWaitForSecondsRealtime(waitForSecondsRealtime);
        waitForSecondsRealtime = null;
    }

    IEnumerator Coroutine2()
    {
        WaitForSecondsRealtime waitForSecondsRealtime = CoroutineCache.GetWaitForSecondsRealtime(3f);

        while (!isExit2)
        {
            yield return new WaitForSeconds(2f);
            Debug.LogWarning("Coroutine2开始" + Time.time);
            yield return waitForSecondsRealtime;
            Debug.LogWarning("Coroutine2结束" + Time.time);
        }

        CoroutineCache.ReleaseWaitForSecondsRealtime(waitForSecondsRealtime);
        waitForSecondsRealtime = null;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isExit = true;

            StartCoroutine(Coroutine2());
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isExit = false;
            isExit2 = true;
            StartCoroutine(Coroutine1());
        }
    }
}