using TMPro;
using UnityEngine;


[DisallowMultipleComponent]
[RequireComponent(typeof(TMP_Text))]
public class TextLocalization : LocalizationComponent
{
    private TMP_Text contentText;

    private void Awake()
    {
        contentText = GetComponent<TMP_Text>();
    }

    protected override void ChangeLanguage()
    {
        contentText.text = LocalizationMgr.Instance.GetLocalizedContent(id);
    }
}
