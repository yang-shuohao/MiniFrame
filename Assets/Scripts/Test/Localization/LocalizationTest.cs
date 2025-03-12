using UnityEngine;

namespace YSH.Framework
{

    public class LocalizationTest : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                LocalizationMgr.Instance.ChangeLanguage(LanguageType.zh_CN);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                LocalizationMgr.Instance.ChangeLanguage(LanguageType.zh_TW);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                LocalizationMgr.Instance.ChangeLanguage(LanguageType.en_US);
            }
        }
    }
}

