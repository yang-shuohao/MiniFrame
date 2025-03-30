using UnityEngine;

namespace YSH.Framework
{
    public class DurationTimer
    {
        /// <summary>
        /// �Ѿ����ŵ�ʱ�� 
        /// </summary>
        private float elapsedTime = 0f;

        /// <summary> 
        /// ��ʱ���ĳ���ʱ�� 
        /// </summary>
        private float duration = 0f;

        /// <summary>
        /// �Ƿ���ͣ 
        /// </summary>
        private bool isPaused = false;

        /// <summary>
        /// ʱ�����ţ�Ĭ��Ϊ 1�������ٶȣ�
        /// </summary>
        private float timeScale = 1f;

        public DurationTimer() { }

        /// <summary>
        /// ��ʼ����ʱ��
        /// </summary>
        /// <param name="duration">��ʱ���ĳ���ʱ��</param>
        public DurationTimer(float duration)
        {
            Reset(duration);
        }

        /// <summary>
        /// ���¼�ʱ��
        /// </summary>
        public void Update()
        {
            if (isPaused || timeScale <= 0) return;

            float deltaTime = Time.unscaledDeltaTime * timeScale;
            elapsedTime += deltaTime;
        }

        /// <summary>
        /// ���ü�ʱ��
        /// </summary>
        public void Reset()
        {
            elapsedTime = 0f;
            isPaused = false;
        }

        /// <summary>
        /// ���µĳ���ʱ�����ü�ʱ��
        /// </summary>
        /// <param name="duration">�µĳ���ʱ��</param>
        public void Reset(float duration)
        {
            this.duration = duration;
            Reset();
        }

        /// <summary>
        /// ��ͣ��ʱ��
        /// </summary>
        public void Pause()
        {
            isPaused = true;
        }

        /// <summary>
        /// �ָ���ʱ��
        /// </summary>
        public void Resume()
        {
            isPaused = false;
        }

        /// <summary>
        /// ����ʱ�����ű���
        /// </summary>
        /// <param name="scale">ʱ�����ű��ʣ�1 = �����ٶȣ�2 = 2���٣�0.5 = ���٣�</param>
        public void SetTimeScale(float scale)
        {
            timeScale = Mathf.Max(0, scale); // ���⸺��
        }

        /// <summary>
        /// ��ȡ��ǰʱ�����ű���
        /// </summary>
        public float GetTimeScale() => timeScale;

        /// <summary>
        /// ����ʱ���Ƿ����
        /// </summary>
        public bool HasElapsed()
        {
            return FloatComparer.IsGreaterThanOrEqualTo(elapsedTime, duration);
        }

        /// <summary>
        /// ��ȡ������ʱ��ı��� [0, 1]
        /// </summary>
        public float GetRatio()
        {
            if (FloatComparer.IsLessThanOrEqualTo(duration, 0))
                return 1.0f;

            return Mathf.Clamp(elapsedTime / duration, 0, 1);
        }

        /// <summary>
        /// ��ȡ������ʱ��
        /// </summary>
        public float GetElapsedTime() => elapsedTime;

        /// <summary>
        /// ������ʱ��
        /// </summary>
        public void EndTimer()
        {
            elapsedTime = duration;
        }

        /// <summary>
        /// ��ȡ��ʱ������ʱ��
        /// </summary>
        public float GetDuration() => duration;

        /// <summary>
        /// ��ȡʣ��ʱ��
        /// </summary>
        public float GetRemainingTime()
        {
            return Mathf.Max(0f, duration - elapsedTime);
        }

        /// <summary>
        /// ��ȡ��ʱ���Ƿ�����ͣ״̬
        /// </summary>
        public bool IsPaused() => isPaused;
    }
}
