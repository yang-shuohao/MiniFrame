using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YSH.Framework;

public class TestEnumerable2 : MonoBehaviour
{
    private void Start()
    {
        InputMgr.Instance.IsEnableInput = true;

        // 监听所有输入事件
        EventMgr.Instance.AddListener<float>(InputEventName.Horizontal, OnHorizontal);
        EventMgr.Instance.AddListener<float>(InputEventName.Vertical, OnVertical);
        EventMgr.Instance.AddListener<KeyCode>(InputEventName.GetKeyDown, OnKeyDown);
        EventMgr.Instance.AddListener<KeyCode>(InputEventName.GetKeyUp, OnKeyUp);
        EventMgr.Instance.AddListener<KeyCode>(InputEventName.GetKey, OnKey);
        EventMgr.Instance.AddListener<int>(InputEventName.GetMouseButtonDown, OnMouseButtonDown);
        EventMgr.Instance.AddListener<int>(InputEventName.GetMouseButtonUp, OnMouseButtonUp);
        EventMgr.Instance.AddListener<int>(InputEventName.GetMouseButton, OnMouseButton);
        EventMgr.Instance.AddListener<float>(InputEventName.MouseScroll, OnMouseScroll);
        EventMgr.Instance.AddListener<Vector3>(InputEventName.MousePosition, OnMousePosition);
        EventMgr.Instance.AddListener<Vector2>(InputEventName.MouseDelta, OnMouseDelta);
        EventMgr.Instance.AddListener<Vector2>(InputEventName.TouchTap, OnTouchTap);
        EventMgr.Instance.AddListener<Vector2>(InputEventName.TouchDown, OnTouchDown);
        EventMgr.Instance.AddListener<Vector2>(InputEventName.TouchDrag, OnTouchDrag);
        EventMgr.Instance.AddListener<Vector2>(InputEventName.TouchUp, OnTouchUp);
        EventMgr.Instance.AddListener<Vector2>(InputEventName.TouchLongPress, OnTouchLongPress);
        EventMgr.Instance.AddListener<float>(InputEventName.TouchPinchZoom, OnPinchZoom);
        EventMgr.Instance.AddListener<Vector3>(InputEventName.Gyro, OnGyro);

        Debug.Log("TestInputLogger: 输入监听器已初始化");
    }

    private void OnDestroy()
    {
        Debug.Log("TestInputLogger: 输入监听器已销毁");
    }



    private void OnHorizontal(float value) => Debug.Log($"Horizontal: {value}");
    private void OnVertical(float value) => Debug.Log($"Vertical: {value}");
    private void OnKeyDown(KeyCode key) => Debug.Log($"Key Down: {key}");
    private void OnKeyUp(KeyCode key) => Debug.Log($"Key Up: {key}");
    private void OnKey(KeyCode key) => Debug.Log($"Key Hold: {key}");
    private void OnMouseButtonDown(int button) => Debug.Log($"Mouse Button Down: {button}");
    private void OnMouseButtonUp(int button) => Debug.Log($"Mouse Button Up: {button}");
    private void OnMouseButton(int button) => Debug.Log($"Mouse Button Hold: {button}");
    private void OnMouseScroll(float value) => Debug.Log($"Mouse Scroll: {value}");
    private void OnMousePosition(Vector3 position) => Debug.Log($"Mouse Position: {position}");
    private void OnMouseDelta(Vector2 delta) => Debug.Log($"Mouse Delta: {delta}");
    private void OnTouchTap(Vector2 position) => Debug.Log($"Touch Tap: {position}");
    private void OnTouchDown(Vector2 position) => Debug.Log($"Touch Down: {position}");
    private void OnTouchDrag(Vector2 position) => Debug.Log($"Touch Drag: {position}");
    private void OnTouchUp(Vector2 position) => Debug.Log($"Touch Up: {position}");
    private void OnTouchLongPress(Vector2 position) => Debug.Log($"Touch Long Press: {position}");
    private void OnPinchZoom(float delta) => Debug.Log($"Pinch Zoom: {delta}");
    private void OnGyro(Vector3 rotation) => Debug.Log($"Gyro: {rotation}");
}
