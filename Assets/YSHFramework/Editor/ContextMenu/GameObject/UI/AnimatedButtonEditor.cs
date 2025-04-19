using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace YSH.Framework.EditorExtensions
{
    public static class AnimatedButtonEditor
    {
        [MenuItem("GameObject/UI/AnimatedButton", false, 11)]
        static void CreateAnimatedButton(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("AnimatedButton");

            // 寻找Canvas
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

            // 添加UI按钮相关组件
            go.AddComponent<RectTransform>();
            go.AddComponent<Image>();
            go.AddComponent<AnimatedButton>();

            Undo.RegisterCreatedObjectUndo(go, "Create AnimatedButton");
            Selection.activeGameObject = go;
        }
    }
}