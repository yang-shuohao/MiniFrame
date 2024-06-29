using UnityEngine;

public class RoundRectGuide : GuideBase
{
    private float halfWidth;
    private float halfHeight;
    private float radius;
    private float scaleHalfWidth;
    private float scaleHalfHeight;
    private float scaleRadius;


    public override void SetMaterialParams(Vector3 center, Vector3[] targetCorners, float scaleTime, float scale)
    {
        base.SetMaterialParams(center, targetCorners, scaleTime, scale);

        halfWidth = (targetCorners[3].x - targetCorners[0].x) / 2;
        halfHeight = (targetCorners[1].y - targetCorners[0].y) / 2;
        radius = halfHeight / 2;

        scaleHalfWidth = scale * halfWidth;
        scaleHalfHeight = scale * halfHeight;
        scaleRadius = scale * radius;

        material.SetFloat("_SliderX", scaleHalfWidth);
        material.SetFloat("_SliderY", scaleHalfHeight);
        material.SetFloat("_Radius", scaleRadius);
    }

    protected override void Update()
    {
        if (durationTimer != null && !durationTimer.HasElapsed())
        {
            durationTimer.Update();

            float ratio = durationTimer.GetRatio();

            material.SetFloat("_SliderX", Mathf.Lerp(scaleHalfWidth, halfWidth, ratio));
            material.SetFloat("_SliderY", Mathf.Lerp(scaleHalfHeight, halfHeight, ratio));
            material.SetFloat("_Radius", Mathf.Lerp(scaleRadius, radius, ratio));
        }
    }
}
