using UnityEngine;
using UnityEngine.UI;

namespace YSH.Framework
{
    [RequireComponent(typeof(Image))]
    public class SoundButton : Button
    {
        //音效名
        public string buttonSFX;

        protected override void Start()
        {
            base.Start();
            onClick.AddListener(PlaySound);
        }

        /// <summary>
        /// 播放点击音效
        /// </summary>
        protected virtual void PlaySound()
        {
            if (buttonSFX != null)
            {
                AudioMgr.Instance.PlaySFX(buttonSFX);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onClick.RemoveListener(PlaySound);
        }
    }
}