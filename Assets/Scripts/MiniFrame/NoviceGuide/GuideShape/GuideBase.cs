using UnityEngine;

public class GuideBase : MonoBehaviour
{
    public Material material;

    protected Vector3 center;

    //缩放时间
    protected float scaleTime;

    //计时器
    protected DurationTimer durationTimer;

    public virtual void SetMaterialParams (Vector3 center, float scaleTime, params float[] materialParams)
    {
        this.center = center;
        this.scaleTime = scaleTime;

        material.SetVector("_Center", center);

        if (!Mathf.Approximately(scaleTime, 0f))
        {
            durationTimer = new DurationTimer(scaleTime);
        }
    }

    protected virtual void Update()
    {

    }
}
