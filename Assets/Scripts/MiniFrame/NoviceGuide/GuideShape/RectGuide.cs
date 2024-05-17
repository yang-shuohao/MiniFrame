
using UnityEngine;

public class RectGuide : GuideBase
{
    private float halfWidth;
    private float halfHeight;
    private float scaleHalfWidth;
    private float scaleHalfHeight;

    public override void SetMaterialParams(Vector3 center, float scaleTime, params float[] materialParams)
    {
        base.SetMaterialParams(center, scaleTime, materialParams);

        if (materialParams.Length == 4)
        {
            halfWidth = materialParams[0];
            halfHeight = materialParams[1];
            scaleHalfWidth = materialParams[2];
            scaleHalfHeight = materialParams[3];
        }
        else
        {
            Debug.LogWarning("Expected exactly 4 parameters for materialParams.");
        }

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
