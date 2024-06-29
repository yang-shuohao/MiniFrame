using UnityEngine;
using DG.Tweening;

public class RedPointShake : MonoBehaviour
{
    private float angle = 60f;
    private float duration = 0.3f;

    private RectTransform rectTransform;

    private Sequence sequence;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        sequence = DOTween.Sequence();
        sequence.Append(rectTransform.DOLocalRotate(new Vector3(0, 0, angle), duration, RotateMode.LocalAxisAdd))
                .Append(rectTransform.DOLocalRotate(new Vector3(0, 0, -angle * 2), duration, RotateMode.LocalAxisAdd))
                .Append(rectTransform.DOLocalRotate(new Vector3(0, 0, angle * 2), duration, RotateMode.LocalAxisAdd))
                .Append(rectTransform.DOLocalRotate(new Vector3(0, 0, -angle * 2), duration, RotateMode.LocalAxisAdd))
                .Append(rectTransform.DOLocalRotate(new Vector3(0, 0, angle), duration, RotateMode.LocalAxisAdd))
                .SetEase(Ease.Linear)
                .AppendInterval(1f)
                .SetLoops(-1);

        sequence.Play();
    }

    private void OnDisable()
    {
        if(sequence != null)
        {
            sequence.Kill();
            sequence = null;
        }
    }
}
