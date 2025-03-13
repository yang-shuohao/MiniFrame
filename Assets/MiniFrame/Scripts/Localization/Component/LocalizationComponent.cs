using UnityEngine;

namespace YSH.Framework
{

    public class LocalizationComponent : MonoBehaviour
    {
        [Header("Excel���ж�ӦID")]
        [SerializeField] protected int id;

        protected virtual void Start()
        {
            EventMgr.Instance.AddListener(LocalizationEventName.ChangeLanguage, ChangeLanguage);
        }

        protected virtual void OnDestroy()
        {
            EventMgr.Instance.RemoveListener(LocalizationEventName.ChangeLanguage, ChangeLanguage);
        }

        protected virtual void ChangeLanguage()
        {

        }
    }
}

