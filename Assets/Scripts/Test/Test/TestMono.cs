

using System.Collections;
using UnityEngine;
using YSH.Framework;

public class TestMono
{
    public TestMono()
    {
        MonoMgr.Instance.AddUpdateListener(OnUpdate);

        MonoMgr.Instance.StartCoroutine(Wait());
    }

    public void OnUpdate()
    {
        Debug.LogWarning(1);
    }

    IEnumerator Wait()
    {
        while(true)
        {
            Debug.LogWarning(2);

            yield return null;
        }
    }
}