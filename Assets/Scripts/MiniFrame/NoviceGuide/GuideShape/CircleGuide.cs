
using UnityEngine;

public class CircleGuide : GuideBase
{
    private float radius;
    private float scaleRadius;

    public override void SetMaterialParams(Vector3 center, Vector3[] targetCorners, float scaleTime, float scale)
    {
        base.SetMaterialParams(center, targetCorners, scaleTime, scale);

        radius = (targetCorners[1].y - targetCorners[0].y) / 2;
        scaleRadius = scale * radius;

        material.SetFloat("_Slider", scaleRadius);
    }

    protected override void Update()
    {
        if(durationTimer != null && !durationTimer.HasElapsed())
        {
            durationTimer.Update();

            float ratio = durationTimer.GetRatio();

            material.SetFloat("_Slider", Mathf.Lerp(scaleRadius, radius, ratio));
        }
    }

}
