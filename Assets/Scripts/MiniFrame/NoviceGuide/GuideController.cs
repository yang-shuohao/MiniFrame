
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GuideController : MonoBehaviour , ICanvasRaycastFilter
{
    //引导区域
    private RectTransform guideArea;

    //遮罩
    private Image mask;

    // 要引导的目标的边界
    private Vector3[] targetCorners = new Vector3[4];

    //引导类
    private RectGuide rectGuide;
    private CircleGuide circleGuide;
    private RoundRectGuide roundRectGuide;

    //引导中心位置
    private Vector3 center;
    //非UI引导时的大小
    private Vector2 size;


    private void Awake()
    {
        mask = transform.GetComponent<Image>();
        guideArea = transform.Find("GuideArea").GetComponent<RectTransform>();
        rectGuide = GetComponent<RectGuide>();
        circleGuide = GetComponent<CircleGuide>();
        roundRectGuide = GetComponent<RoundRectGuide>();
    }

    //UI引导
    public void Guide(RectTransform clickAbleRectTransform, GuideType guideType, float scale = 1f, float scaleTime = 0f)
    {
        clickAbleRectTransform.GetWorldCorners(targetCorners);
        ConvertWorldToLocalPoints(clickAbleRectTransform.GetComponentInParent<Canvas>(), true);
        SpawnGuide(guideType, scale, scaleTime);
        SetGuideArea(clickAbleRectTransform.position, clickAbleRectTransform.sizeDelta);
    }

    //非UI引导
    public void Guide(Bounds bounds, GuideType guideType, float scale = 1f, float scaleTime = 0f)
    {
        GetWorldCorners(bounds, targetCorners);
        ConvertWorldToLocalPoints(GetComponentInParent<Canvas>(), false);
        SpawnGuide(guideType, scale, scaleTime);
        SetGuideArea(center, size);
    }


    private void ConvertWorldToLocalPoints(Canvas canvas, bool isUI)
    {
        for (int i = 0; i < targetCorners.Length; i++)
        {
            targetCorners[i] = WorldPointToLocalPointInRectangle(canvas, targetCorners[i], isUI);
        }
    }

    private void SpawnGuide(GuideType guideType, float scale, float scaleTime)
    {
        center.x = targetCorners[0].x + (targetCorners[3].x - targetCorners[0].x) / 2;
        center.y = targetCorners[0].y + (targetCorners[1].y - targetCorners[0].y) / 2;
        center.z = 0f;

        size = new Vector2(targetCorners[3].x - targetCorners[0].x, targetCorners[1].y - targetCorners[0].y);

        switch(guideType)
        {
            case GuideType.Rect:
                mask.material = rectGuide.material;
                rectGuide.SetMaterialParams(center, targetCorners, scaleTime, scale);
                break;
            case GuideType.Circle:
                mask.material = circleGuide.material;
                circleGuide.SetMaterialParams(center, targetCorners, scaleTime, scale);
                break;
            case GuideType.RoundRect:
                mask.material = roundRectGuide.material;
                roundRectGuide.SetMaterialParams(center, targetCorners, scaleTime, scale);
                break;
        }
    }

    private void SetGuideArea(Vector3 position, Vector2 size)
    {
        guideArea.transform.position = position;
        guideArea.sizeDelta = size;
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        //在目标范围内做事件渗透
        return !RectTransformUtility.RectangleContainsScreenPoint(guideArea, sp, eventCamera);
    }

    //世界坐标转Canvas局部坐标
    public Vector2 WorldPointToLocalPointInRectangle(Canvas canvas, Vector3 world, bool isUI)
    {
        //注意UI和非UI使用不同方法
        Vector2 screenPoint = isUI ? RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, world) : Camera.main.WorldToScreenPoint(world);

        //此模式需要置空摄像机
        if(canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            canvas.worldCamera = null;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, canvas.worldCamera, out Vector2 localPoint);

        return localPoint;
    }

    //获取Bounds的四个角世界位置
    private void GetWorldCorners(Bounds bounds, Vector3[] fourCornersArray)
    {
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
        fourCornersArray[0] = new Vector3(min.x, min.y, 0);
        fourCornersArray[1] = new Vector3(min.x, max.y, 0);
        fourCornersArray[2] = new Vector3(max.x, max.y, 0);
        fourCornersArray[3] = new Vector3(max.x, min.y, 0);
    }
}
