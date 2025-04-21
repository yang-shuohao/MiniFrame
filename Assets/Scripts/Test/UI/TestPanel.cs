
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YSH.Framework;


public class TestPanel : BaseUI
{
    private Image imgTest;
    private TMP_Text txtTest;
    private Button btnTest;
    private Toggle tglTest;
    private RawImage rimgTest;

    protected override void Awake()
    {
        base.Awake();
        imgTest = GetControl<Image>(nameof(imgTest));
        txtTest = GetControl<TMP_Text>(nameof(txtTest));
        btnTest = GetControl<Button>(nameof(btnTest));
        tglTest = GetControl<Toggle>(nameof(tglTest));
        rimgTest = GetControl<RawImage>(nameof(rimgTest));
    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "btnTest":
                break;
        }
    }
}
