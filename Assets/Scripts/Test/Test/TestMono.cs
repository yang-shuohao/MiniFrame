

using UnityEngine;
using YSH.Framework;
using DG.Tweening;
using YSH.Framework.Utils;
using UnityEngine.UI;


public class TestMono : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Start()
    {
        LogMgr.Instance.isPrintErrorMsgOnScreen = true;

        GMMgr.Instance.EnableGM();

        sr.sortingLayerID = 1;
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