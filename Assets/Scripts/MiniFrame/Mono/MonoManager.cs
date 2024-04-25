
using MiniFrame.Base;
using UnityEngine.Events;


namespace MiniFrame.Mono
{
    /// <summary>
    /// Monoπ‹¿Ì∆˜
    /// </summary>
    public class MonoManager : MonoSingleton<MonoManager>
    {
        private event UnityAction updateEvent;

        private void Update()
        {
            updateEvent?.Invoke();
        }

        public void AddUpdateListener(UnityAction action)
        {
            updateEvent += action;
        }

        public void RemoveUpdateListener(UnityAction action)
        {
            updateEvent -= action;
        }
    }
}

