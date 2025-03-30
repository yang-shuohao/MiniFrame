using UnityEngine;

namespace YSH.Framework
{
    public class DurationTimer
    {
        /// <summary>
        /// 已经流逝的时间 
        /// </summary>
        private float elapsedTime = 0f;

        /// <summary> 
        /// 计时器的持续时间 
        /// </summary>
        private float duration = 0f;

        /// <summary>
        /// 是否暂停 
        /// </summary>
        private bool isPaused = false;

        /// <summary>
        /// 时间缩放，默认为 1（正常速度）
        /// </summary>
        private float timeScale = 1f;

        public DurationTimer() { }

        /// <summary>
        /// 初始化计时器
        /// </summary>
        /// <param name="duration">计时器的持续时间</param>
        public DurationTimer(float duration)
        {
            Reset(duration);
        }

        /// <summary>
        /// 更新计时器
        /// </summary>
        public void Update()
        {
            if (isPaused || timeScale <= 0) return;

            float deltaTime = Time.unscaledDeltaTime * timeScale;
            elapsedTime += deltaTime;
        }

        /// <summary>
        /// 重置计时器
        /// </summary>
        public void Reset()
        {
            elapsedTime = 0f;
            isPaused = false;
        }

        /// <summary>
        /// 以新的持续时间重置计时器
        /// </summary>
        /// <param name="duration">新的持续时间</param>
        public void Reset(float duration)
        {
            this.duration = duration;
            Reset();
        }

        /// <summary>
        /// 暂停计时器
        /// </summary>
        public void Pause()
        {
            isPaused = true;
        }

        /// <summary>
        /// 恢复计时器
        /// </summary>
        public void Resume()
        {
            isPaused = false;
        }

        /// <summary>
        /// 设置时间缩放比例
        /// </summary>
        /// <param name="scale">时间缩放倍率（1 = 正常速度，2 = 2倍速，0.5 = 半速）</param>
        public void SetTimeScale(float scale)
        {
            timeScale = Mathf.Max(0, scale); // 避免负数
        }

        /// <summary>
        /// 获取当前时间缩放倍率
        /// </summary>
        public float GetTimeScale() => timeScale;

        /// <summary>
        /// 检查计时器是否结束
        /// </summary>
        public bool HasElapsed()
        {
            return FloatComparer.IsGreaterThanOrEqualTo(elapsedTime, duration);
        }

        /// <summary>
        /// 获取已流逝时间的比例 [0, 1]
        /// </summary>
        public float GetRatio()
        {
            if (FloatComparer.IsLessThanOrEqualTo(duration, 0))
                return 1.0f;

            return Mathf.Clamp(elapsedTime / duration, 0, 1);
        }

        /// <summary>
        /// 获取已流逝时间
        /// </summary>
        public float GetElapsedTime() => elapsedTime;

        /// <summary>
        /// 结束计时器
        /// </summary>
        public void EndTimer()
        {
            elapsedTime = duration;
        }

        /// <summary>
        /// 获取计时器的总时长
        /// </summary>
        public float GetDuration() => duration;

        /// <summary>
        /// 获取剩余时间
        /// </summary>
        public float GetRemainingTime()
        {
            return Mathf.Max(0f, duration - elapsedTime);
        }

        /// <summary>
        /// 获取计时器是否处于暂停状态
        /// </summary>
        public bool IsPaused() => isPaused;
    }
}
