using System;
using UnityEngine;

public class CTest : MonoBehaviour
{
    private void Start()
    {
        int? a = new int();
        a = 1;
        int b = (int)a;
    }
}
