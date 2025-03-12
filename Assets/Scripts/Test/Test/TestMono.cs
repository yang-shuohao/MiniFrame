

using UnityEngine;
using YSH.Framework;

public class TestMono : MonoSingleton<TestMono>
{
    public void Test()
    {
        Debug.LogWarning(1);
    }
}