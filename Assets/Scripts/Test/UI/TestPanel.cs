
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YSH.Framework;

public class TestPanel : BaseUI
{
    private TMP_Text txtCoint;
    private Image imgIcon;
    private Button btnRun2;
    private Button btnPlay;
    private Button btnTest;
    

    protected override void Awake()
    {
        base.Awake();
        txtCoint = GetControl<TMP_Text>(nameof(txtCoint));
        imgIcon = GetControl<Image>(nameof(imgIcon));
        btnRun2 = GetControl<Button>(nameof(btnRun2));
        btnPlay = GetControl<Button>(nameof(btnPlay));
        btnTest = GetControl<Button>(nameof(btnTest));
    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "btnRun2":
                break;
            case "btnPlay":
                break;
            case "btnTest":
                break;
        }
    }
}
