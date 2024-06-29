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
        // ��ȡ��Ļ���ĸ��߽�
        Vector3[] screenCorners = new Vector3[4];
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, Vector2.zero, canvas.worldCamera, out screenCorners[0]); // Bottom-left
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, new Vector2(Screen.width, 0), canvas.worldCamera, out screenCorners[1]); // Bottom-right
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, new Vector2(Screen.width, Screen.height), canvas.worldCamera, out screenCorners[2]); // Top-right
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, new Vector2(0, Screen.height), canvas.worldCamera, out screenCorners[3]); // Top-left

        // ��ȡRectTransform���ĸ��߽�
        Vector3[] objectCorners = new Vector3[4];
        rectTransform.GetWorldCorners(objectCorners);

        // ����Ƿ񳬳���Ļ

        //�Ƿ񳬳������Ļ
        bool isOutLeft = destroyOnLeftExit && objectCorners[2].x < screenCorners[0].x;
        //�Ƿ񳬳��ұ���Ļ
        bool isOutRight = destroyOnRightExit && objectCorners[1].x > screenCorners[1].x;
        //�Ƿ񳬳��ϱ���Ļ
        bool isOutTop = destroyOnTopExit && objectCorners[3].y > screenCorners[3].y;
        //�Ƿ񳬳��±���Ļ
        bool isOutBottom = destroyOnBottomExit && objectCorners[1].y < screenCorners[0].y;

        return isOutLeft || isOutRight || isOutTop || isOutBottom;
    }
}
