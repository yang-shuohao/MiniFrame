using UnityEngine;

public static class FloatComparer
{
    // �ж������������Ƿ����
    public static bool AreEqual(float a, float b, float tolerance = 0.00001f)
    {
        return Mathf.Abs(a - b) < tolerance;
    }

    // ʹ�� Approximate �ж������������Ƿ����
    public static bool ApproximatelyEqual(float a, float b)
    {
        return Mathf.Approximately(a, b);
    }

    // �жϸ����� a �Ƿ���ڸ����� b
    public static bool IsGreaterThan(float a, float b, float tolerance = 0.00001f)
    {
        return (a - b) > tolerance;
    }

    // �жϸ����� a �Ƿ�С�ڸ����� b
    public static bool IsLessThan(float a, float b, float tolerance = 0.00001f)
    {
        return (b - a) > tolerance;
    }

    // �жϸ����� a �Ƿ���ڵ��ڸ����� b
    public static bool IsGreaterThanOrEqualTo(float a, float b, float tolerance = 0.00001f)
    {
        return IsGreaterThan(a, b, tolerance) || AreEqual(a, b, tolerance);
    }

    // �жϸ����� a �Ƿ�С�ڵ��ڸ����� b
    public static bool IsLessThanOrEqualTo(float a, float b, float tolerance = 0.00001f)
    {
        return IsLessThan(a, b, tolerance) || AreEqual(a, b, tolerance);
    }
}
