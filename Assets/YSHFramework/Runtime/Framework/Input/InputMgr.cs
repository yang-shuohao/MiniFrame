using System;
using UnityEngine;

namespace YSH.Framework
{
    public class InputMgr : Singleton<InputMgr>
    {
        public bool IsEnableInput { get; set; } = false;

        private float touchStartTime;
        private Vector2 lastTouchPos;
        private bool isDragging;

        private float lastTapTime;
        private Vector2 lastTapPosition;
        private int tapCount;

        public InputMgr()
        {
            MonoMgr.Instance.AddUpdateListener(Update);
        }

        private void Update()
        {
            if (IsEnableInput)
            {
                CheckInput();
            }
        }

        private void CheckInput()
        {
            // ¼ì²âË®Æ½/´¹Ö±ÖáÊäÈë
            CheckAxis();

            //¼ì²â¼üÅÌ°´¼ü
            CheckKeys();

            //¼ì²âÊó±ê°´¼ü
            CheckMouseButton();

            // ¼ì²âÊó±ê¹öÂÖ
            CheckMouseScroll();

            // ¼ì²âÊó±êÒÆ¶¯
            CheckMouseMove();

            //¼ì²â´¥Ãþ
            CheckTouchInput();

            //¼ì²âÍÓÂÝÒÇ
            CheckGyro();

        }

        #region ¼üÅÌÊó±ê

        private void CheckAxis()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (!Mathf.Approximately(horizontal, 0f))
            {
                EventMgr.Instance.Dispatcher<float>(InputEventName.Horizontal, horizontal);
            }
            if (!Mathf.Approximately(vertical, 0f))
            {
                EventMgr.Instance.Dispatcher<float>(InputEventName.Vertical, vertical);
            }
        }

        private void CheckKeys()
        {
            foreach (var item in Enum.GetValues(typeof(KeyCode)))
            {
                CheckKeysCode((KeyCode)item);
            }
        }

        void CheckKeysCode(KeyCode key)
        {
            if (Input.GetKeyDown(key))
            {
                EventMgr.Instance.Dispatcher<KeyCode>(InputEventName.GetKeyDown, key);
            }
            if (Input.GetKeyUp(key))
            {
                EventMgr.Instance.Dispatcher<KeyCode>(InputEventName.GetKeyUp, key);
            }
            if (Input.GetKey(key))
            {
                EventMgr.Instance.Dispatcher<KeyCode>(InputEventName.GetKey, key);
            }
        }

        private void CheckMouseButton()
        {
            for (int i = 0; i <= 2; i++)
            {
                if (Input.GetMouseButtonDown(i))
                {
                    EventMgr.Instance.Dispatcher<int>(InputEventName.GetMouseButtonDown, i);
                }
                if (Input.GetMouseButtonUp(i))
                {
                    EventMgr.Instance.Dispatcher<int>(InputEventName.GetMouseButtonUp, i);
                }
                if (Input.GetMouseButton(i))
                {
                    EventMgr.Instance.Dispatcher<int>(InputEventName.GetMouseButton, i);
                }
            }
        }

        private void CheckMouseScroll()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.0001f)
            {
                EventMgr.Instance.Dispatcher<float>(InputEventName.MouseScroll, scroll);
            }
        }

        private void CheckMouseMove()
        {
            Vector3 mousePosition = Input.mousePosition;
            EventMgr.Instance.Dispatcher(InputEventName.MousePosition, mousePosition);

            float mouseDeltaX = Input.GetAxis("Mouse X");
            float mouseDeltaY = Input.GetAxis("Mouse Y");

            if (Mathf.Abs(mouseDeltaX) > 0.0001f || Mathf.Abs(mouseDeltaY) > 0.0001f)
            {
                EventMgr.Instance.Dispatcher(InputEventName.MouseDelta, new Vector2(mouseDeltaX, mouseDeltaY));
            }
        }

        #endregion

        #region ´¥Ãþ

        private void CheckTouchInput()
        {
            int touchCount = Input.touchCount;
            if (touchCount == 0) return;

            if (touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                HandleSingleTouch(touch);
            }
            else if (touchCount >= 2)
            {
                HandleMultiTouch();
            }
        }

        // ´¦Àíµ¥Ö¸´¥Ãþ£¨µã»÷¡¢³¤°´¡¢Ë«»÷¡¢ÍÏ×§£©
        private void HandleSingleTouch(Touch touch)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartTime = Time.time;
                    lastTouchPos = touch.position;
                    isDragging = false;

                    // ´¦ÀíË«»÷
                    if (Time.time - lastTapTime < 0.3f && Vector2.Distance(lastTapPosition, touch.position) < 30f)
                    {
                        tapCount++;
                        if (tapCount == 2)
                        {
                            EventMgr.Instance.Dispatcher(InputEventName.TouchDoubleTap, touch.position);
                        }
                        tapCount = 0;
                    }
                    else
                    {
                        tapCount = 1;
                    }

                    lastTapTime = Time.time;
                    lastTapPosition = touch.position;

                    EventMgr.Instance.Dispatcher(InputEventName.TouchDown, touch.position);
                    break;

                case TouchPhase.Moved:
                    if (Vector2.Distance(touch.position, lastTouchPos) > 10f)
                    {
                        isDragging = true;
                        EventMgr.Instance.Dispatcher(InputEventName.TouchDrag, touch.position);
                    }
                    break;

                case TouchPhase.Ended:
                    float touchDuration = Time.time - touchStartTime;

                    if (!isDragging)
                    {
                        if (touchDuration < 0.2f)
                        {
                            EventMgr.Instance.Dispatcher(InputEventName.TouchTap, touch.position);
                        }
                        else
                        {
                            EventMgr.Instance.Dispatcher(InputEventName.TouchLongPress, touch.position);
                        }
                    }
                    EventMgr.Instance.Dispatcher(InputEventName.TouchUp, touch.position);
                    break;
            }
        }

        // ´¦Àí¶àÖ¸´¥Ãþ£¨Ëõ·Å£©
        private void HandleMultiTouch()
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float currentTouchDeltaMag = (touch1.position - touch2.position).magnitude;

            float deltaMagnitudeDiff = currentTouchDeltaMag - prevTouchDeltaMag;
            EventMgr.Instance.Dispatcher(InputEventName.TouchPinchZoom, deltaMagnitudeDiff);
        }

        private void CheckGyro()
        {
            if (SystemInfo.supportsGyroscope)
            {
                Vector3 rotation = Input.gyro.rotationRate;
                EventMgr.Instance.Dispatcher(InputEventName.Gyro, rotation);
            }
        }

        #endregion
    }
}
