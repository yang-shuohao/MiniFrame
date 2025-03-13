using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YSH.Framework;

public class TestEnumerable2 : MonoBehaviour
{
    private void Update()
    {
         if(Input.GetKeyDown(KeyCode.Space))
        {
            new TestMono();
        }
    }


}
