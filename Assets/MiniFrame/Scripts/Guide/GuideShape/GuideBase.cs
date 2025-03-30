using UnityEngine;

using YSH.Framework;

public class GuideBase : MonoBehaviour
{
    public Material material;

    protected Vector3 center;

    //缩放时间
    protected float scaleTime;
    protected float scale;

    //计时器
    protected DurationTimer durationTimer;

    public virtual void SetMaterialParams (Vector3 center, Vector3[] targetCorners, float scaleTime, float scale)
    {
        this.center = center;
        this.scaleTime = scaleTime;
        this.scale = scale;

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
