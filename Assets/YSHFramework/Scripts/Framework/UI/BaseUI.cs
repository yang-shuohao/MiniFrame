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
            FindChildrenControl<Button>();
            FindChildrenControl<Image>();
            FindChildrenControl<TMP_Text>();
            FindChildrenControl<Toggle>();
            FindChildrenControl<Slider>();
            FindChildrenControl<ScrollRect>();
            FindChildrenControl<TMP_InputField>();
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

                //Button
                if (controls[i] is Button)
                {
                    (controls[i] as Button).onClick.AddListener(() =>
                    {
                        OnClick(objName);
                    });
                }
                //Toggle
                else if (controls[i] is Toggle)
                {
                    (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                    {
                        OnValueChanged(objName, value);
                    });
                }
                //TODO �������������Ҫ��UI
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
        /// ��ť���
        /// </summary>
        protected virtual void OnClick(string btnName)
        {

        }

        /// <summary>
        /// ��ѡ��ѡ��ֵ�ı�
        /// </summary>
        protected virtual void OnValueChanged(string toggleName, bool value)
        {

        }
    }
}

