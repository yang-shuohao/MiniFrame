using DG.Tweening;
using UnityEngine;
using YSH.Framework.Attributes;

namespace YSH.Framework
{
    public class UIAnimator : MonoBehaviour
    {
        public enum AnimationType
        {
            None,
            Rotate,
            Scale
        }

        public AnimationType animationType = AnimationType.None;

        public float duration = 1f;

        [ShowIf("animationType", AnimationType.Rotate)]
        public float rotateSpeed = 90f;

        [ShowIf("animationType", AnimationType.Scale)]
        public float scaleMultiplier = 1.2f;

        private RectTransform rectTransform;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        void OnEnable()
        {
            PlayAnimation();
        }

        public void PlayAnimation()
        {
            if (rectTransform == null) return;

            switch (animationType)
            {
                case AnimationType.Rotate:
                    rectTransform.DORotate(new Vector3(0, 0, 360f), 360f / rotateSpeed, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1)
                    .SetLink(gameObject, LinkBehaviour.KillOnDisable);
                    break;

                case AnimationType.Scale:
                    rectTransform.DOScale(Vector3.one * scaleMultiplier, duration)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetLink(gameObject, LinkBehaviour.KillOnDisable);
                    break;
            }
        }
    }
}