

using UnityEngine;
using YSH.Framework;
using DG.Tweening;
using YSH.Framework.Utils;
using UnityEngine.UI;
using YSH.Framework.Attributes;

public class TestMono : MonoBehaviour
{
    [ProgressBar("Mana", 100, EColor.Red)]
    public float mana = 80f;

    [SortingLayer()]
    public string sortingLayer;

    [HorizontalLine(EColor.Red, 1f, 2f, "我是小亮", TextAlignment.Center, 12, EColor.Green)]
    public int a;

    public bool showAdvanced;

    [ShowIf("showAdvanced == true")]
    public float advancedValue;


    [InspectorButton("打印日志", InspectorButtonAttribute.Mode.Always)]
    private void PrintLog()
    {
        Debug.Log("按钮点击，日志输出！");
    }
}