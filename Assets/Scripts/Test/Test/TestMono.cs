

using UnityEngine;
using YSH.Framework;
using DG.Tweening;
using YSH.Framework.Utils;


public class TestMono : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Start()
    {
        LogMgr.Instance.isPrintErrorMsgOnScreen = true;

        GMMgr.Instance.EnableGM();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
           
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
 
        }
    }
}