
using System;
using UnityEngine;

namespace YSH.Framework.Utils
{
    public static class SortingLayerUtils
    {
        /// <summary>
        /// 是否包含排序层
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

            throw new Exception($"Sorting Layer '{layerName}' 不存在！请在 Unity 编辑器的 Project Settings > Tags and Layers 里手动添加！");
        }
    }
}
