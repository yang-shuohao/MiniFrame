using UnityEngine;

public static class FloatComparer
{
    // 判断两个浮点数是否相等
    public static bool AreEqual(float a, float b, float tolerance = 0.00001f)
    {
        return Mathf.Abs(a - b) < tolerance;
    }

    // 使用 Approximate 判断两个浮点数是否相等
    public static bool ApproximatelyEqual(float a, float b)
    {
        return Mathf.Approximately(a, b);
    }

    // 判断浮点数 a 是否大于浮点数 b
    public static bool IsGreaterThan(float a, float b, float tolerance = 0.00001f)
    {
        return (a - b) > tolerance;
    }

    // 判断浮点数 a 是否小于浮点数 b
    public static bool IsLessThan(float a, float b, float tolerance = 0.00001f)
    {
        return (b - a) > tolerance;
    }

    // 判断浮点数 a 是否大于等于浮点数 b
    public static bool IsGreaterThanOrEqualTo(float a, float b, float tolerance = 0.00001f)
    {
        return IsGreaterThan(a, b, tolerance) || AreEqual(a, b, tolerance);
    }

    // 判断浮点数 a 是否小于等于浮点数 b
    public static bool IsLessThanOrEqualTo(float a, float b, float tolerance = 0.00001f)
    {
        return IsLessThan(a, b, tolerance) || AreEqual(a, b, tolerance);
    }
}
