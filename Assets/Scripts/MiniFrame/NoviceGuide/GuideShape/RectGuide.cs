
using UnityEngine;

public class RectGuide : GuideBase
{
    private float halfWidth;
    private float halfHeight;
    private float scaleHalfWidth;
    private float scaleHalfHeight;

    public override void SetMaterialParams(Vector3 center, Vector3[] targetCorners, float scaleTime, float scale)
    {
        base.SetMaterialParams(center, targetCorners, scaleTime, scale);

        halfWidth = (targetCorners[3].x - targetCorners[0].x) / 2;
        halfHeight = (targetCorners[1].y - targetCorners[0].y) / 2;

        scaleHalfWidth = scale * halfWidth;
        scaleHalfHeight = scale * halfHeight;

        material.SetFloat("_SliderX", scaleHalfWidth);
        material.SetFloat("_SliderY", scaleHalfHeight);
    }


    protected override void Update()
    {
        if (durationTimer != null && !durationTimer.HasElapsed())
        {
            durationTimer.Update();

            float ratio = durationTimer.GetRatio();

            material.SetFloat("_SliderX", Mathf.Lerp(scaleHalfWidth, halfWidth, ratio));
            material.SetFloat("_SliderY", Mathf.Lerp(scaleHalfHeight, halfHeight, ratio));        
        }
    }
}
