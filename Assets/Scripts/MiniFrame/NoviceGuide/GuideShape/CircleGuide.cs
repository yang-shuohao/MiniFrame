
using UnityEngine;

public class CircleGuide : GuideBase
{
    private float radius;
    private float scaleRadius;

    public override void SetMaterialParams(Vector3 center, float scaleTime, params float[] materialParams)
    {
        base.SetMaterialParams(center, scaleTime, materialParams);

        if (materialParams.Length == 2)
        {
            radius = materialParams[0];
            scaleRadius = materialParams[1];
        }
        else
        {
            Debug.LogWarning("Expected exactly 2 parameters for materialParams.");
        }

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
