

using UnityEngine;
using YSH.Framework;
using DG.Tweening;
using YSH.Framework.Utils;
using UnityEngine.UI;
using YSH.Framework.Attributes;

public class TestMono : MonoBehaviour
{
    private void Start()
    {

    }

    private void Update()
    {

    }

    [InspectorButton("打印日志", InspectorButtonAttribute.Mode.Always)]
    private void PrintLog()
    {
        Debug.Log("按钮点击，日志输出！");
    }

    [InspectorButton("仅在运行时可用", InspectorButtonAttribute.Mode.PlayModeOnly)]
    private void RuntimeOnly()
    {
        Debug.Log("运行时按钮被点击！");
    }

    [InspectorButton("仅在编辑器可用", InspectorButtonAttribute.Mode.EditorOnly)]
    private void EditorOnly()
    {
        Debug.Log("编辑器下点击！");
    }

    [InspectorButton("Test", InspectorButtonAttribute.Mode.Always)]
    private void Test()
    {

    }
}