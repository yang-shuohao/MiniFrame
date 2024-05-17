
using UnityEngine;

public class NoviceGuideTest : MonoBehaviour
{
    GuideController guideController;

    void Start()
    {
        guideController = transform.GetComponent<GuideController>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            guideController.Guide(GameObject.Find("Button").GetComponent<RectTransform>(), GuideType.RoundRect, 3f, 1f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            guideController.Guide(GameObject.Find("Square").GetComponent<SpriteRenderer>().bounds, GuideType.Circle, 3f, 1f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            guideController.Guide(GameObject.Find("Button2").GetComponent<RectTransform>(), GuideType.Rect, 3f, 1f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            guideController.Guide(GameObject.Find("Button3").GetComponent<RectTransform>(), GuideType.RoundRect);
        }
    }
}
