

using UnityEngine;
using UnityEngine.Events;

namespace YSH.Framework
{

    /// <summary>
    /// 本地化管理器
    /// </summary>
    public class LocalizationMgr : Singleton<LocalizationMgr>
    {
        private LanguageContainer languageContainer;

        private LanguageType languageType;

        private void InitLanguageContainer()
        {
            BinaryDataMgr.Instance.LoadTable<LanguageContainer, Language>();
            languageContainer = BinaryDataMgr.Instance.GetTable<LanguageContainer>();
        }

        public void ChangeLanguage(LanguageType languageType)
        {
            this.languageType = languageType;

            if (languageContainer == null)
            {
                InitLanguageContainer();
            }

            EventMgr.Instance.EventDispatcher(LocalizationEventName.ChangeLanguage);
        }

        public string GetLocalizedContent(int id)
        {
            switch (languageType)
            {
                case LanguageType.zh_CN:
                    return languageContainer.dataDic[id].zh_CN;
                case LanguageType.zh_TW:
                    return languageContainer.dataDic[id].zh_TW;
                case LanguageType.en_US:
                    return languageContainer.dataDic[id].en_US;
            }

            return null;
        }

        public void GetLocalizedContent(int id, UnityAction<Sprite> callBack)
        {
            string imagePath = null;
            switch (languageType)
            {
                case LanguageType.zh_CN:
                    imagePath = languageContainer.dataDic[id].zh_CN;
                    break;
                case LanguageType.zh_TW:
                    imagePath = languageContainer.dataDic[id].zh_TW;
                    break;
                case LanguageType.en_US:
                    imagePath = languageContainer.dataDic[id].en_US;
                    break;
            }

            ResMgr.Instance.LoadAssetAsync<Sprite>(imagePath, ResMgr.Instance.resLoadType, (res) =>
            {
                callBack?.Invoke(res);
            });
        }
    }

}