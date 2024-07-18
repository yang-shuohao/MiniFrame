using UnityEngine;
using System.Collections.Generic;

public static class Utility
{
    /// <summary>
    /// 查找离当前对象最近的物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="current"></param>
    /// <param name="objects"></param>
    /// <returns></returns>
    public static T GetNearestObject<T>(Transform current, List<T> objects) where T : Component
    {
        if (current == null || objects == null || objects.Count == 0)
        {
            return null;
        }

        T nearest = null;
        float nearestDistance = Mathf.Infinity;

        foreach (T obj in objects)
        {
            float distance = Vector3.Distance(current.position, obj.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = obj;
            }
        }

        return nearest;
    }
}
