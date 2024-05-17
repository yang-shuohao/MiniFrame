using UnityEngine;

public class RoundRectGuide : GuideBase
{
    private float halfWidth;
    private float halfHeight;
    private float radius;
    private float scaleHalfWidth;
    private float scaleHalfHeight;
    private float scaleRadius;


    public override void SetMaterialParams(Vector3 center, float scaleTime, params float[] materialParams)
    {
        base.SetMaterialParams(center, scaleTime, materialParams);

        if (materialParams.Length == 6)
        {
            halfWidth = materialParams[0];
            halfHeight = materialParams[1];
            radius = materialParams[2];
            scaleHalfWidth = materialParams[3];
            scaleHalfHeight = materialParams[4];
            scaleRadius = materialParams[5];
        }
        else
        {
            Debug.LogWarning("Expected exactly 6 parameters for materialParams.");
        }

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
