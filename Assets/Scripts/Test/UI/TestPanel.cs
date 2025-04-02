
using UnityEngine.UI;
using YSH.Framework;

public class TestPanel : BaseUI
{
    private Image imgIcon;
    private Button btnRun2;
    private Button btnPlay;
    

    protected override void Awake()
    {
        base.Awake();
        imgIcon = GetControl<Image>(nameof(imgIcon));
        btnRun2 = GetControl<Button>(nameof(btnRun2));
        btnPlay = GetControl<Button>(nameof(btnPlay));
    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "btnRun2":
                break;
            case "btnPlay":
                break;
        }
    }
}
