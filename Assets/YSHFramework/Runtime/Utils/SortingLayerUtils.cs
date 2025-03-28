
using System;
using UnityEngine;

namespace YSH.Framework.Utils
{
    public static class SortingLayerUtils
    {
        /// <summary>
        /// �Ƿ���������
        /// </summary>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public static bool IsContains(string layerName)
        {
            int length = SortingLayer.layers.Length;
            for (int i = 0; i < length; i++)
            {
                if (SortingLayer.layers[i].name == layerName)
                {
                    return true;
                }
            }

            throw new Exception($"Sorting Layer '{layerName}' �����ڣ����� Unity �༭���� Project Settings > Tags and Layers ���ֶ���ӣ�");
        }
    }
}
