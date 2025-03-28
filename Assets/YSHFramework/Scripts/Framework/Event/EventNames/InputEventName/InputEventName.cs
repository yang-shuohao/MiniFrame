
/// <summary>
/// 输入事件名
/// </summary>
public static class InputEventName
{
    //轴
    public const string Horizontal = "Horizontal";
    public const string Vertical = "Vertical";

    //键盘
    public const string GetKeyDown = "GetKeyDown";
    public const string GetKeyUp = "GetKeyUp";
    public const string GetKey = "GetKey";

    //鼠标
    public const string GetMouseButtonDown = "GetMouseButtonDown";
    public const string GetMouseButtonUp = "GetMouseButtonUp";
    public const string GetMouseButton = "GetMouseButton";
    public const string MouseScroll = "MouseScroll";
    public const string MouseDelta = "MouseDelta";
    public const string MousePosition = "MousePosition";

    // 触摸
    public const string TouchDown = "TouchDown";               // 触摸按下
    public const string TouchUp = "TouchUp";                   // 触摸抬起
    public const string TouchTap = "TouchTap";                 // 轻触（单击）
    public const string TouchDoubleTap = "TouchDoubleTap";     // 双击
    public const string TouchLongPress = "TouchLongPress";     // 长按
    public const string TouchDrag = "TouchDrag";               // 拖拽
    public const string TouchPinchZoom = "TouchPinchZoom";     // 双指缩放
    public const string Gyro = "Gyro";                         // 陀螺仪
}
