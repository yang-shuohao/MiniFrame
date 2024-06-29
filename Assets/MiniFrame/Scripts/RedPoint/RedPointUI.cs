using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RedPointUI : MonoBehaviour, IPointerClickHandler
{
    //路径
    public string Path;

    //文本
    private TMP_Text redPointNumText;

    //红点
    private GameObject redPointObject;


    void Start()
    {
        redPointNumText = GetComponentInChildren<TMP_Text>();
        redPointNumText.text = null;

        TreeNode node = RedPointMgr.Instance.AddListener(Path, RedPointCallback);
        gameObject.name = node.FullPath;
    }

    private void RedPointCallback(int value)
    {
        redPointNumText.text = value.ToString();

        if (value > 0)
        {
            //创建显示红点
            if (redPointObject == null)
            {
                SpawnRedPointObject();
            }
            else
            {
                if (!redPointObject.activeSelf)
                {
                    redPointObject.SetActive(true);
                }
            }
        }
        else
        {
            //隐藏红点
            if (redPointObject != null)
            {
                Destroy(redPointObject);
            }
        }
    }

    //生成红点
    private void SpawnRedPointObject()
    {
        ResMgr.Instance.LoadAssetAsync<GameObject>("RedPoint", ResMgr.Instance.resLoadType, (res) =>
        {
            if(redPointObject == null)
            {
                redPointObject = Instantiate(res);
                redPointObject.transform.SetParent(this.transform);

                Image image = redPointObject.GetComponent<Image>();

                RectTransform redPointRectTransform = redPointObject.GetComponent<RectTransform>();
                redPointRectTransform.anchoredPosition = Vector2.zero;
                redPointRectTransform.sizeDelta = new Vector2(image.sprite.texture.width, image.sprite.texture.height);
                redPointRectTransform.anchorMin = Vector2.one;
                redPointRectTransform.anchorMax = Vector2.one;
                redPointRectTransform.localScale = Vector3.one;
            }

        });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int value = RedPointMgr.Instance.GetValue(Path);

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            RedPointMgr.Instance.ChangeValue(Path, value + 1);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            RedPointMgr.Instance.ChangeValue(Path, Mathf.Clamp(value - 1, 0, value));
        }
    }
}
