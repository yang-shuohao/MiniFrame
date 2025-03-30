using UnityEngine;

namespace YSH.Framework.Extensions
{
    /// <summary>
    /// �ݹ����� GameObject ���������Ӷ���Ĳ㼶��
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// �ݹ����� GameObject ���������Ӷ���Ĳ㼶��
        /// </summary>
        /// <param name="gameObject">��Ҫ���Ĳ㼶�� GameObject��</param>
        /// <param name="layer">Ŀ��㼶��</param>
        public static void SetLayerRecursive(this GameObject gameObject, int layer)
        {
            if (gameObject == null) return;

            // �����ǰ�㼶�Ѿ���Ŀ��㼶�������������ⲻ��Ҫ�ĸ�ֵ
            if (gameObject.layer != layer)
            {
                gameObject.layer = layer;
            }

            // ʹ�� Transform �������������壬���� GetComponent ����
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetLayerRecursive(layer);
            }
        }
    }
}