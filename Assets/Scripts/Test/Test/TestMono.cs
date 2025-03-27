

using System.Collections;
using UnityEngine;
using YSH.Framework;

public class TestMono : MonoBehaviour
{
    private void Start()
    {
        //new GameObject("Test").AddComponent<TestEnumerable2>().InitTest();
        UIMgr.Instance.GetPanel<TestPanel>("Test");
    }
}