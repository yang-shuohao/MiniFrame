
using UnityEngine;

public class DurationTimer
{
    //已经经过的时间
    private float polledTime;
    //总的时间
    private float durationTime;


    public DurationTimer(float durationTime)
    {
        Reset(durationTime);
    }

    public void Update()
    {
        this.polledTime += Time.deltaTime;
    }

    public void Reset()
    {
        this.polledTime = 0;
    }

    public void Reset(float durationTime)
    {
        Reset();
        this.durationTime = durationTime;
    }

    //是否已经结束
    public bool HasElapsed()
    {
        return FloatComparer.IsGreaterThanOrEqualTo(this.polledTime, this.durationTime);
    }

    public float GetRatio()
    {
        //防止除0
        if (FloatComparer.IsLessThanOrEqualTo(this.durationTime, 0))
        {
            return 1.0f;
        }

        float ratio = this.polledTime / this.durationTime;
        return Mathf.Clamp(ratio, 0, 1);
    }

    public float GetPolledTime()
    {
        return this.polledTime;
    }

    public void EndTimer()
    {
        this.polledTime = this.durationTime;
    }

    public float GetDurationTime()
    {
        return this.durationTime;
    }

    public float GetLeftTime()
    {
        return durationTime - polledTime;
    }

    // 获取 "00" 格式的字符串
    public string GetLeftTimeString00()
    {
        int seconds = Mathf.FloorToInt(GetLeftTime()) % 60;
        return string.Format("{0:00}", seconds);
    }

    // 获取 "00:00" 格式的字符串
    public string GetLeftTimeString0000()
    {
        float leftTime = GetLeftTime();

        int minutes = Mathf.FloorToInt(leftTime / 60);
        int seconds = Mathf.FloorToInt(leftTime) % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // 获取 "00:00:00" 格式的字符串
    public string GetLeftTimeString000000()
    {
        float leftTime = GetLeftTime();

        int hours = Mathf.FloorToInt(leftTime / 3600);
        int minutes = Mathf.FloorToInt((leftTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(leftTime) % 60;
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    //根据剩余时间自动匹配格式
    public string GetLeftTimeString()
    {
        float leftTime = GetLeftTime();

        int hours = Mathf.FloorToInt(leftTime / 3600);
        int minutes = Mathf.FloorToInt((leftTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(leftTime) % 60;

        if (hours > 0)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
        else if (minutes > 0)
        {
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            return string.Format("{0}", seconds);
        }
    }
}
