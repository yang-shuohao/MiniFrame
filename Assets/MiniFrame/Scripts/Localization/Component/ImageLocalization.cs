using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Image))]
public class ImageLocalization : LocalizationComponent
{
    private Image contentIMage;

    private void Awake()
    {
        contentIMage = GetComponent<Image>();
    }

    protected override void ChangeLanguage()
    {
        LocalizationMgr.Instance.GetLocalizedContent(id, UpdateImage);
    }

    private void UpdateImage(Sprite sprite)
    {
        contentIMage.sprite = sprite;
    }
}
