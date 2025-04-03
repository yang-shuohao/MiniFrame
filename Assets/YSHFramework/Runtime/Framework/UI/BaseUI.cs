using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YSH.Framework
{
    public class BaseUI : MonoBehaviour
    {
        //�洢��ǰUI�����пؼ�
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
        /// �ҵ��Ӷ���Ķ�Ӧ�ؼ�
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

                // ��ť�¼�
                if (controls[i] is Button button)
                {
                    button.onClick.AddListener(() => OnClick(objName));
                }
                // Toggle �¼�
                else if (controls[i] is Toggle toggle)
                {
                    toggle.onValueChanged.AddListener(value => OnValueChanged(objName, value));
                }
                // Slider �¼�
                else if (controls[i] is Slider slider)
                {
                    slider.onValueChanged.AddListener(value => OnSliderValueChanged(objName, value));
                }
                // TMP_Dropdown �¼�
                else if (controls[i] is TMP_Dropdown dropdown)
                {
                    dropdown.onValueChanged.AddListener(value => OnDropdownValueChanged(objName, value));
                }
                // TMP_InputField �¼�
                else if (controls[i] is TMP_InputField inputField)
                {
                    inputField.onValueChanged.AddListener(value => OnInputFieldValueChanged(objName, value));
                    inputField.onEndEdit.AddListener(value => OnInputFieldEndEdit(objName, value));
                }
                // ScrollRect ��һ�㲻������������Լ��ϣ�
                else if (controls[i] is ScrollRect scrollRect)
                {
                    scrollRect.onValueChanged.AddListener(value => OnScrollValueChanged(objName, value));
                }
            }
        }

        /// <summary>
        /// �õ���Ӧ���ֵĶ�Ӧ�ؼ��ű�
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
        /// ��ť����¼�
        /// </summary>
        protected virtual void OnClick(string btnName) { }

        /// <summary>
        /// Toggle ֵ�ı��¼�
        /// </summary>
        protected virtual void OnValueChanged(string toggleName, bool value) { }

        /// <summary>
        /// Slider ֵ�ı��¼�
        /// </summary>
        protected virtual void OnSliderValueChanged(string sliderName, float value) { }

        /// <summary>
        /// Dropdown ѡ��ı��¼�
        /// </summary>
        protected virtual void OnDropdownValueChanged(string dropdownName, int value) { }

        /// <summary>
        /// InputField ����仯�¼�
        /// </summary>
        protected virtual void OnInputFieldValueChanged(string inputFieldName, string value) { }

        /// <summary>
        /// InputField ��������¼�
        /// </summary>
        protected virtual void OnInputFieldEndEdit(string inputFieldName, string value) { }

        /// <summary>
        /// ScrollRect �����¼�����ѡ��
        /// </summary>
        protected virtual void OnScrollValueChanged(string scrollName, Vector2 value) { }
    }
}

