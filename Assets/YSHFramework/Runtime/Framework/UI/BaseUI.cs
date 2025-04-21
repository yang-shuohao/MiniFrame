using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YSH.Framework
{
    public class BaseUI : MonoBehaviour
    {
        //存储当前UI的所有控件
        private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

        protected virtual void Awake()
        {
            FindChildrenControl<Image>();         // img
            FindChildrenControl<TMP_Text>();      // txt
            FindChildrenControl<Button>();        // btn
            FindChildrenControl<Toggle>();        // tgl
            FindChildrenControl<RawImage>();      // rimg
        }

        /// <summary>
        /// 找到子对象的对应控件
        /// </summary>
        private void FindChildrenControl<T>() where T : UIBehaviour
        {
            T[] controls = this.GetComponentsInChildren<T>(true);
            for (int i = 0; i < controls.Length; ++i)
            {
                string objName = controls[i].gameObject.name;
                if (controlDic.ContainsKey(objName))
                {
                    controlDic[objName].Add(controls[i]);
                }
                else
                {
                    controlDic.Add(objName, new List<UIBehaviour>() { controls[i] });
                }

                // Button事件
                if (controls[i] is Button button)
                {
                    button.onClick.AddListener(() => OnClick(objName));
                }
                // Toggle 事件
                else if (controls[i] is Toggle toggle)
                {
                    toggle.onValueChanged.AddListener(value => OnValueChanged(objName, value));
                }
            }
        }

        /// <summary>
        /// 得到对应名字的对应控件脚本
        /// </summary>
        protected T GetControl<T>(string controlName) where T : UIBehaviour
        {
            if (controlDic.ContainsKey(controlName))
            {
                for (int i = 0; i < controlDic[controlName].Count; ++i)
                {
                    if (controlDic[controlName][i] is T)
                    {
                        return controlDic[controlName][i] as T;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        protected virtual void OnClick(string btnName) { }

        /// <summary>
        /// Toggle 值改变事件
        /// </summary>
        protected virtual void OnValueChanged(string toggleName, bool value) { }
    }
}

