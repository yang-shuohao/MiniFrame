
using UnityEngine;
using UnityEngine.Tilemaps;

namespace YSH.Framework.Extensions
{
    public static class TilemapExtensions
    {
        /// <summary>
        /// �ж����������Ƿ���Tile
        /// </summary>
        /// <param name="tilemap">Ŀ��Tilemap</param>
        /// <param name="worldPosition">��������</param>
        /// <returns>�����λ����Tile������true�����򷵻�false</returns>
        public static bool HasTile(this Tilemap tilemap, Vector3 worldPosition)
        {
            // ����������ת��Ϊ��������
            Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
            // ʹ��Tilemap��HasTile�����ж��Ƿ���Tile
            return tilemap.HasTile(cellPosition);
        }
    }
}