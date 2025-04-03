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
            FindChildrenControl<Image>();//img
            FindChildrenControl<TMP_Text>();//txt
            FindChildrenControl<RawImage>();//rimg
            FindChildrenControl<Toggle>();//tog
            FindChildrenControl<Slider>();//sli
            FindChildrenControl<ScrollRect>();//sr
            FindChildrenControl<Button>();//btn
            FindChildrenControl<TMP_Dropdown>();//dd
            FindChildrenControl<TMP_InputField>();//if
        }

        /// <summary>
        /// 找到子对象的对应控件
        /// </summary>
        private void FindChildrenControl<T>() where T : UIBehaviour
        {
            T[] controls = this.GetComponentsInChildren<T>();
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

                // 按钮事件
                if (controls[i] is Button button)
                {
                    button.onClick.AddListener(() => OnClick(objName));
                }
                // Toggle 事件
                else if (controls[i] is Toggle toggle)
                {
                    toggle.onValueChanged.AddListener(value => OnValueChanged(objName, value));
                }
                // Slider 事件
                else if (controls[i] is Slider slider)
                {
                    slider.onValueChanged.AddListener(value => OnSliderValueChanged(objName, value));
                }
                // TMP_Dropdown 事件
                else if (controls[i] is TMP_Dropdown dropdown)
                {
                    dropdown.onValueChanged.AddListener(value => OnDropdownValueChanged(objName, value));
                }
                // TMP_InputField 事件
                else if (controls[i] is TMP_InputField inputField)
                {
                    inputField.onValueChanged.AddListener(value => OnInputFieldValueChanged(objName, value));
                    inputField.onEndEdit.AddListener(value => OnInputFieldEndEdit(objName, value));
                }
                // ScrollRect （一般不会监听，但可以加上）
                else if (controls[i] is ScrollRect scrollRect)
                {
                    scrollRect.onValueChanged.AddListener(value => OnScrollValueChanged(objName, value));
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

        /// <summary>
        /// Slider 值改变事件
        /// </summary>
        protected virtual void OnSliderValueChanged(string sliderName, float value) { }

        /// <summary>
        /// Dropdown 选项改变事件
        /// </summary>
        protected virtual void OnDropdownValueChanged(string dropdownName, int value) { }

        /// <summary>
        /// InputField 输入变化事件
        /// </summary>
        protected virtual void OnInputFieldValueChanged(string inputFieldName, string value) { }

        /// <summary>
        /// InputField 输入结束事件
        /// </summary>
        protected virtual void OnInputFieldEndEdit(string inputFieldName, string value) { }

        /// <summary>
        /// ScrollRect 滑动事件（可选）
        /// </summary>
        protected virtual void OnScrollValueChanged(string scrollName, Vector2 value) { }
    }
}

