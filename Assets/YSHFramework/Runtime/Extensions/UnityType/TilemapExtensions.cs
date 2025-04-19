
using UnityEngine;
using UnityEngine.Tilemaps;

namespace YSH.Framework.Extensions
{
    public static class TilemapExtensions
    {
        /// <summary>
        /// 判断世界坐标是否有Tile
        /// </summary>
        /// <param name="tilemap">目标Tilemap</param>
        /// <param name="worldPosition">世界坐标</param>
        /// <returns>如果该位置有Tile，返回true；否则返回false</returns>
        public static bool HasTile(this Tilemap tilemap, Vector3 worldPosition)
        {
            // 将世界坐标转换为网格坐标
            Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
            // 使用Tilemap的HasTile方法判断是否有Tile
            return tilemap.HasTile(cellPosition);
        }
    }
}