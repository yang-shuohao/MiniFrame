using UnityEngine;

namespace YSH.Framework
{

    public class LocalizationComponent : MonoBehaviour
    {
        [Header("Excel表中对应ID")]
        [SerializeField] protected int id;

        protected virtual void Start()
        {
            EventMgr.Instance.AddEventListener(LocalizationEventName.ChangeLanguage, ChangeLanguage);
        }

        protected virtual void OnDestroy()
        {
            EventMgr.Instance.RemoveEventListener(LocalizationEventName.ChangeLanguage, ChangeLanguage);
        }

        protected virtual void ChangeLanguage()
        {

        }
    }
}

