using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace YSH.Framework
{
    [RequireComponent(typeof(Image))]
    public class SoundToggle : Toggle
    {
        public string toggleSFX;
        protected bool wasClicked = false;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            wasClicked = true;
        }

        protected override void Start()
        {
            base.Start();
            onValueChanged.AddListener(OnToggleChanged);
        }

        protected virtual void OnToggleChanged(bool isOn)
        {
            if (wasClicked)
            {
                wasClicked = false;

                if (!string.IsNullOrEmpty(toggleSFX))
                {
                    AudioMgr.Instance.PlaySFX(toggleSFX);
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onValueChanged.RemoveListener(OnToggleChanged);
        }
    }
}
