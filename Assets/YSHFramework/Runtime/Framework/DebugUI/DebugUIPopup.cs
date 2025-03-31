
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugUIPopup
{
    //������ʾ
    private GameObject tipsGO;
    private Image imgTips;
    private TMP_Text txtTips;
    private Tween tipsTween;

    public DebugUIPopup(Transform canvasTrans)
    {
        CreateTipsUI(canvasTrans);
    }

    //��ʾ����UI������Ϣ
    public void ShowPopup(string msg, float duration = 2f)
    {
        if (tipsTween != null && tipsTween.IsActive())
        {
            tipsTween.Kill();
        }

        tipsGO.SetActive(true);
        tipsGO.transform.SetAsLastSibling();

        txtTips.text = msg;

        tipsTween = imgTips.DOFade(1f, duration).SetLink(tipsGO, LinkBehaviour.KillOnDisable).OnComplete(() =>
        {
            tipsTween = null;
            tipsGO.SetActive(false);
        });
    }

    private void CreateTipsUI(Transform canvasTrans)
    {
        //������ʾ���
        tipsGO = new GameObject("TipsGO");
        imgTips = tipsGO.AddComponent<Image>();
        imgTips.color = Color.green;
        RectTransform imgTipsRT = tipsGO.transform as RectTransform;
        tipsGO.transform.SetParent(canvasTrans, false);
        imgTipsRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500f);
        imgTipsRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100f);
        imgTipsRT.anchorMin = Vector2.one * 0.5f;
        imgTipsRT.anchorMax = Vector2.one * 0.5f;
        imgTipsRT.anchoredPosition = Vector2.zero;

        //������ʾ�ı�
        GameObject txtTipsObj = new GameObject(nameof(txtTips));
        txtTips = txtTipsObj.AddComponent<TextMeshProUGUI>();
        txtTips.fontSize = 40f;
        txtTips.color = Color.red;
        txtTips.alignment = TextAlignmentOptions.Center;
        txtTipsObj.transform.SetParent(tipsGO.transform, false);
        RectTransform txtTipsRT = txtTips.transform as RectTransform;
        txtTipsRT.anchorMin = Vector2.zero;
        txtTipsRT.anchorMax = Vector2.one;
        txtTipsRT.offsetMin = Vector2.zero;
        txtTipsRT.offsetMax = Vector2.zero;
    }
}
