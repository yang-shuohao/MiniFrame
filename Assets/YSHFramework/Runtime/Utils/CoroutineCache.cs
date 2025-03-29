using System.Collections.Generic;
using UnityEngine;

namespace YSH.Framework.Utils
{
    public static class CoroutineCache
    {
        // ���� WaitForSeconds������ GC
        private static readonly Dictionary<float, WaitForSeconds> waitForSecondsCache = new Dictionary<float, WaitForSeconds>(GameComparer.DefaultFloatEqualityComparer);

        // ����ع��� WaitForSecondsRealtime����ֹ waitTime �������޸�
        private static readonly Dictionary<float, Stack<WaitForSecondsRealtime>> waitForSecondsRealtimePool = new Dictionary<float, Stack<WaitForSecondsRealtime>>(GameComparer.DefaultFloatEqualityComparer);

        // WaitForEndOfFrame �� WaitForFixedUpdate ֱ��ʹ�� Unity �ڲ�����
        private static readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        private static readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        /// <summary>
        /// ��ȡ WaitForSeconds���ɸ���
        /// </summary>
        public static WaitForSeconds GetWaitForSeconds(float seconds)
        {
            if (waitForSecondsCache.TryGetValue(seconds, out var wait))
            {
                return wait;
            }
            wait = new WaitForSeconds(seconds);
            waitForSecondsCache[seconds] = wait;
            return wait;
        }

        /// <summary>
        /// ��ȡ WaitForSecondsRealtime���Ӷ���ع�����ֹ���ô���
        /// </summary>
        public static WaitForSecondsRealtime GetWaitForSecondsRealtime(float seconds)
        {
            if (waitForSecondsRealtimePool.TryGetValue(seconds, out var stack) && stack.Count > 0)
            {
                WaitForSecondsRealtime waitForSecondsRealtime = stack.Pop();
                waitForSecondsRealtime.Reset();
                return waitForSecondsRealtime;
            }
            return new WaitForSecondsRealtime(seconds);
        }

        /// <summary>
        /// �ͷ� WaitForSecondsRealtime����������
        /// </summary>
        public static void ReleaseWaitForSecondsRealtime(WaitForSecondsRealtime wait)
        {
            float time = wait.waitTime;
            if (!waitForSecondsRealtimePool.TryGetValue(time, out var stack))
            {
                stack = new Stack<WaitForSecondsRealtime>();
                waitForSecondsRealtimePool[time] = stack;
            }
            stack.Push(wait);
        }

        /// <summary>
        /// ��ȡ WaitForEndOfFrame��������
        /// </summary>
        public static WaitForEndOfFrame GetWaitForEndOfFrame()
        {
            return waitForEndOfFrame;
        }

        /// <summary>
        /// ��ȡ WaitForFixedUpdate��������
        /// </summary>
        public static WaitForFixedUpdate GetWaitForFixedUpdate()
        {
            return waitForFixedUpdate;
        }
    }
}