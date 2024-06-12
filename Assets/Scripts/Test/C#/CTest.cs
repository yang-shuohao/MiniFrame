using System;
using UnityEngine;

public class CTest : MonoBehaviour
{
    private void Start()
    {
        LogMgr.Instance.LogError("123");
        GMGUIMgr.Instance.isShowGMUI = true;
    }
}
