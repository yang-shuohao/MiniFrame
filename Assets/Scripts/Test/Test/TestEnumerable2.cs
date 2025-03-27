using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YSH.Framework;

public class TestEnumerable2 : MonoBehaviour
{
    private void Awake()
    {
        Debug.LogWarning("Awake");
    }

    private void OnEnable()
    {
        Debug.LogWarning("OnEnable");
    }

    public void InitTest()
    {
        Debug.LogWarning("InitTest");
    }

    private void Start()
    {
       
    }
  
}
