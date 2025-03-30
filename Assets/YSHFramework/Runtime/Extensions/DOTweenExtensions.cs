

using DG.Tweening;
using System;
using UnityEngine;

namespace YSH.Framework.Extensions
{
    public static class DOTweenExtensions
    {
        /// <summary>
        /// �Ժ㶨�ٶ��ƶ� Transform ��Ŀ��λ�ã�֧�� 2D/3D����
        /// </summary>
        public static Tween DOMoveAtSpeed(this Transform target, Vector3 targetPos, float speed)
        {
            if (speed <= 0) return null;

            float distance = Vector3.Distance(target.position, targetPos);
            float duration = distance / speed;

            return target.DOMove(targetPos, duration)
                         .SetEase(Ease.Linear)
                         .SetLink(target.gameObject, LinkBehaviour.KillOnDisable);
        }

        /// <summary>
        /// �Ժ㶨�ٶ��ƶ� Transform ��ָ�� X ���ꡣ
        /// </summary>
        public static Tween DOMoveXAtSpeed(this Transform target, float targetX, float speed)
        {
            return target.DOMoveAtSpeed(new Vector3(targetX, target.position.y, target.position.z), speed);
        }

        #region �����ƶ�

        /// <summary>
        /// �� Transform �ر����������ƶ����Զ�������Ƶ㣩��
        /// </summary>
        public static Tween DOMoveCurve(this Transform target, Vector3 startPoint, Vector3 endPoint, float duration, Func<Vector3, Vector3, Vector3> onCalculateControlPoint = null, bool isLocal = false)
        {
            Vector3 controlPoint = (onCalculateControlPoint ?? DefaultControlPoint)(startPoint, endPoint);

            Vector3[] path = { startPoint, controlPoint, endPoint };
            var tween = isLocal
                ? target.DOLocalPath(path, duration, PathType.CatmullRom, PathMode.Full3D)
                : target.DOPath(path, duration, PathType.CatmullRom, PathMode.Full3D);

            return tween.SetEase(Ease.InOutQuad);
        }

        /// <summary>
        /// �� RectTransform �ض��α����������ƶ���
        /// </summary>
        public static Tween DOMoveCurve(this RectTransform target, Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint, float duration)
        {
            return DOTween.To(() => 0f, t => target.anchoredPosition = BezierPoint(t, startPoint, controlPoint, endPoint), 1f, duration)
                          .SetEase(Ease.InOutQuad);
        }

        /// <summary>
        /// ������α��������ߵĵ㡣
        /// </summary>
        private static Vector3 BezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            return u * u * p0 + 2 * u * t * p1 + t * t * p2;
        }

        /// <summary>
        /// Ĭ�ϵ����߿��Ƶ���㣬ʹ���߸���Ȼ��
        /// </summary>
        private static Vector3 DefaultControlPoint(Vector3 start, Vector3 end)
        {
            // ���������յ���е�
            Vector3 mid = (start + end) / 2;

            // �������տ��Ƶ�
            Vector3 controlPoint = new Vector3(start.x, mid.y, mid.z);

            return controlPoint;
        }

        #endregion
    }
}

