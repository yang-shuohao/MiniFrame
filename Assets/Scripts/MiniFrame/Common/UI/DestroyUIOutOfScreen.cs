using UnityEngine;

public class DestroyUIOutOfScreen : MonoBehaviour
{
    public bool destroyOnTopExit = true;
    public bool destroyOnBottomExit = true;
    public bool destroyOnLeftExit = true;
    public bool destroyOnRightExit = true;

    private RectTransform rectTransform;
    private Canvas canvas;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        if (IsOutOfScreen())
        {
            Destroy(gameObject);
        }
    }

    private bool IsOutOfScreen()
    {
        // 获取屏幕的四个边界
        Vector3[] screenCorners = new Vector3[4];
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, Vector2.zero, canvas.worldCamera, out screenCorners[0]); // Bottom-left
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, new Vector2(Screen.width, 0), canvas.worldCamera, out screenCorners[1]); // Bottom-right
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, new Vector2(Screen.width, Screen.height), canvas.worldCamera, out screenCorners[2]); // Top-right
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, new Vector2(0, Screen.height), canvas.worldCamera, out screenCorners[3]); // Top-left

        // 获取RectTransform的四个边界
        Vector3[] objectCorners = new Vector3[4];
        rectTransform.GetWorldCorners(objectCorners);

        // 检查是否超出屏幕

        //是否超出左边屏幕
        bool isOutLeft = destroyOnLeftExit && objectCorners[2].x < screenCorners[0].x;
        //是否超出右边屏幕
        bool isOutRight = destroyOnRightExit && objectCorners[1].x > screenCorners[1].x;
        //是否超出上边屏幕
        bool isOutTop = destroyOnTopExit && objectCorners[3].y > screenCorners[3].y;
        //是否超出下边屏幕
        bool isOutBottom = destroyOnBottomExit && objectCorners[1].y < screenCorners[0].y;

        return isOutLeft || isOutRight || isOutTop || isOutBottom;
    }
}
