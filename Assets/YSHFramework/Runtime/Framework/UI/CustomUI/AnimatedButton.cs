using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace YSH.Framework
{
    [RequireComponent(typeof(Image))]
    public class AnimatedButton : SoundButton
    {
        private float scaleFactor = 1.1f;
        private float duration = 0.1f;

        private Vector3 originalScale;

        private Tween tween;

        protected override void Start()
        {
            base.Start();
            originalScale = transform.localScale;
            onClick.AddListener(PlayAnimation);
        }

        private void PlayAnimation()
        {
            if (tween != null && tween.IsActive())
            {
                tween.Kill();
            }

            transform.DOScale(originalScale * scaleFactor, duration).SetLoops(2, LoopType.Yoyo).SetLink(gameObject, LinkBehaviour.KillOnDisable).OnKill(() =>
            {
                transform.localScale = originalScale;
                tween = null;
            });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onClick.RemoveListener(PlayAnimation);
        }
    }
}