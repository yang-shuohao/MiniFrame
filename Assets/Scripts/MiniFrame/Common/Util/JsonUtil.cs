using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class JsonUtil
{
    /// <summary>
    /// �ж�json���Ƿ������Ӧ��key
    /// </summary>
    /// <param name="data"></param>
    /// <param name="keys"></param>
    /// <returns></returns>
    public static bool IsDataContainkeys(JsonData data, params string[] keys)
    {
        string ret = string.Empty;
        return IsDataContainkeys(data, out ret, keys);
    }

    /// <summary>
    /// �ж�json���Ƿ������Ӧ��key
    /// </summary>
    /// <param name="data"></param>
    /// <param name="ret"> ���ز����ڵ�key�ַ��� </param>
    /// <param name="keys"></param>
    /// <returns></returns>
    public static bool IsDataContainkeys(JsonData data, out string ret, params string[] keys)
    {
        ret = string.Empty;
        if (keys == null || keys.Length == 0) return false;
        for (int i = 0; i < keys.Length; ++i)
        {
            if (!((IDictionary)data).Contains(keys[i]))
            {
                ret += keys[i];
                if (i != keys.Length - 1)
                    ret += ", ";
            }
        }

        return string.IsNullOrEmpty(ret);
    }

    /// <summary>
    /// jsonתVector3
    /// ��ʽ{"x":111,"y":22.22,"z":33}
    /// </summary>
    public static Vector3 ConvertJsonToVector3(JsonData data)
    {
        Vector3 ret = Vector3.zero;

        if (data == null)
        {
            return ret;
        }

        if (IsDataContainkeys(data, "x"))
        {
            ret.x = float.Parse(data["x"].ToString());
        }

        if (IsDataContainkeys(data, "y"))
        {
            ret.y = float.Parse(data["y"].ToString());
        }

        if (IsDataContainkeys(data, "z"))
        {
            ret.z = float.Parse(data["z"].ToString());
        }

        return ret;
    }

    /// <summary>
    /// JsonתVector3
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static Vector3 ConvertJsonToVector3(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return Vector3.zero;
        }

        JsonData data = JsonMapper.ToObject(json);
        return ConvertJsonToVector3(data);
    }

    /// <summary>
    /// Vector3תjson
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public static string ConvertVector3ToJsonStr(Vector3 vec3)
    {
        var sbr = new StringBuilder();
        sbr.Append(@"{'x':");
        sbr.Append(vec3.x);
        sbr.Append(@",'y':");
        sbr.Append(vec3.y);
        sbr.Append(@",'z':");
        sbr.Append(vec3.z);
        sbr.Append("}");
        return sbr.ToString();
    }

    /// <summary>
    /// jsonתVector2
    /// ��ʽ{"x":111,"y":22.22}
    /// </summary>
    public static Vector2 ConvertJsonToVector2(JsonData data)
    {
        Vector2 ret = Vector2.zero;

        if (data == null)
        {
            return ret;
        }

        if (IsDataContainkeys(data, "x"))
        {
            ret.x = float.Parse(data["x"].ToString());
        }

        if (IsDataContainkeys(data, "y"))
        {
            ret.y = float.Parse(data["y"].ToString());
        }

        return ret;
    }

    /// <summary>
    /// Json����תΪVector2
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public static Vector2 ConvertJsonToVector2(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return Vector2.zero;
        }

        JsonData data = JsonMapper.ToObject(json);
        return ConvertJsonToVector2(data);
    }

    /// <summary>
    /// Vector2תΪjson
    /// </summary>
    /// <param name="vec2"></param>
    /// <returns></returns>
    public static string ConvertVector2ToJsonStr(Vector2 vec2)
    {
        var sbr = new StringBuilder();
        sbr.Append(@"{'x':");
        sbr.Append(vec2.x);
        sbr.Append(@",'y':");
        sbr.Append(vec2.y);
        sbr.Append("}");
        return sbr.ToString();
    }

    /// <summary>
    /// ���Ի�ȡkeyֵ��Ӧ��value
    /// </summary>
    public static string TryGetValue(JsonData data, string key, string defaultValue)
    {
        if (IsDataContainkeys(data, key))
        {
            return data[key].ToString();
        }
        return defaultValue;
    }

    /// <summary>
    /// jsonת��ΪList����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static List<T> ConvetJsonToList<T>(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return new List<T>();
        }

        return JsonMapper.ToObject<List<T>>(json);
    }

}