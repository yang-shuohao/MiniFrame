using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace YSH.Framework.EditorExtensions
{
    public static class SoundToggleEditor
    {
        [MenuItem("GameObject/UI/SoundToggle", false, 13)]
        static void CreateSoundToggle(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("SoundToggle");

            // 设置父级（判断是否有 Canvas）
            GameObject parent = menuCommand.context as GameObject;
            if (parent != null && parent.GetComponentInParent<Canvas>() != null)
            {
                go.transform.SetParent(parent.transform, false);
            }
            else
            {
                Canvas canvas = Object.FindObjectOfType<Canvas>();
                if (canvas == null)
                {
                    GameObject canvasGO = new GameObject("Canvas", typeof(Canvas));
                    canvas = canvasGO.GetComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvasGO.AddComponent<CanvasScaler>();
                    canvasGO.AddComponent<GraphicRaycaster>();
                }
                go.transform.SetParent(canvas.transform, false);
            }

            // 添加组件
            var rectTransform = go.AddComponent<RectTransform>();
            var image = go.AddComponent<Image>();
            var toggle = go.AddComponent<SoundToggle>();
            rectTransform.sizeDelta = new Vector2(50, 50);
            image.color = new Color(1f, 1f, 1f, 1f);

            // 创建 Checkmark 子物体
            GameObject checkmark = new GameObject("Checkmark");
            checkmark.transform.SetParent(go.transform, false);
            var checkmarkRect = checkmark.AddComponent<RectTransform>();
            checkmarkRect.anchorMin = new Vector2(0.5f,0.5f);
            checkmarkRect.anchorMax = new Vector2(0.5f, 0.5f);
            checkmarkRect.sizeDelta = rectTransform.sizeDelta;

            var checkmarkImage = checkmark.AddComponent<Image>();
            checkmarkImage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Checkmark.psd");
            checkmarkImage.color = Color.black;

            // 设置 Toggle 的引用
            toggle.targetGraphic = image;
            toggle.graphic = checkmarkImage;
            toggle.isOn = true;

            // 注册
            Undo.RegisterCreatedObjectUndo(go, "Create SoundToggle");
            Selection.activeGameObject = go;
        }
    }
}