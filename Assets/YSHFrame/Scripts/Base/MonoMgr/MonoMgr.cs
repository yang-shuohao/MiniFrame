
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace YSH.Framework
{
    public class MonoMgr : MonoSingleton<MonoMgr>
    {
        private event UnityAction updateEvent;
        private event UnityAction fixedUpdateEvent;
        private event UnityAction lateUpdateEvent;

        private void Update()
        {
            updateEvent?.Invoke();
        }

        private void FixedUpdate()
        {
            fixedUpdateEvent?.Invoke();
        }

        private void LateUpdate()
        {
            lateUpdateEvent?.Invoke();
        }

        // 添加 Update 监听
        public void AddUpdateListener(UnityAction fun) => updateEvent += fun;
        public void RemoveUpdateListener(UnityAction fun) => updateEvent -= fun;

        // 添加 FixedUpdate 监听
        public void AddFixedUpdateListener(UnityAction fun) => fixedUpdateEvent += fun;
        public void RemoveFixedUpdateListener(UnityAction fun) => fixedUpdateEvent -= fun;

        // 添加 LateUpdate 监听
        public void AddLateUpdateListener(UnityAction fun) => lateUpdateEvent += fun;
        public void RemoveLateUpdateListener(UnityAction fun) => lateUpdateEvent -= fun;

        // 开启协程
        public Coroutine StartMonoCoroutine(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }

        // 停止协程
        public void StopMonoCoroutine(Coroutine routine)
        {
            if (routine != null)
                StopCoroutine(routine);
        }

        // 停止所有协程
        public void StopAllMonoCoroutines()
        {
            StopAllCoroutines();
        }
    }
}


