using System.Collections.Generic;
using UnityEngine;

namespace YSH.Framework.Utils
{
    public static class CoroutineCache
    {
        // 缓存 WaitForSeconds，避免 GC
        private static readonly Dictionary<float, WaitForSeconds> waitForSecondsCache = new Dictionary<float, WaitForSeconds>(GameComparer.DefaultFloatEqualityComparer);

        // 对象池管理 WaitForSecondsRealtime，防止 waitTime 被复用修改
        private static readonly Dictionary<float, Stack<WaitForSecondsRealtime>> waitForSecondsRealtimePool = new Dictionary<float, Stack<WaitForSecondsRealtime>>(GameComparer.DefaultFloatEqualityComparer);

        // WaitForEndOfFrame 和 WaitForFixedUpdate 直接使用 Unity 内部单例
        private static readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        private static readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        /// <summary>
        /// 获取 WaitForSeconds，可复用
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
        /// 获取 WaitForSecondsRealtime，从对象池管理，防止复用错误
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
        /// 释放 WaitForSecondsRealtime，存入对象池
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
        /// 获取 WaitForEndOfFrame（单例）
        /// </summary>
        public static WaitForEndOfFrame GetWaitForEndOfFrame()
        {
            return waitForEndOfFrame;
        }

        /// <summary>
        /// 获取 WaitForFixedUpdate（单例）
        /// </summary>
        public static WaitForFixedUpdate GetWaitForFixedUpdate()
        {
            return waitForFixedUpdate;
        }
    }
}