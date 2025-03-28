
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

        // ��� Update ����
        public void AddUpdateListener(UnityAction fun) => updateEvent += fun;
        public void RemoveUpdateListener(UnityAction fun) => updateEvent -= fun;

        // ��� FixedUpdate ����
        public void AddFixedUpdateListener(UnityAction fun) => fixedUpdateEvent += fun;
        public void RemoveFixedUpdateListener(UnityAction fun) => fixedUpdateEvent -= fun;

        // ��� LateUpdate ����
        public void AddLateUpdateListener(UnityAction fun) => lateUpdateEvent += fun;
        public void RemoveLateUpdateListener(UnityAction fun) => lateUpdateEvent -= fun;

        // ����Э��
        public Coroutine StartMonoCoroutine(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }

        // ֹͣЭ��
        public void StopMonoCoroutine(Coroutine routine)
        {
            if (routine != null)
                StopCoroutine(routine);
        }

        // ֹͣ����Э��
        public void StopAllMonoCoroutines()
        {
            StopAllCoroutines();
        }
    }
}


