using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace YSH.Framework.EditorExtensions
{
    public static class SoundButtonEditor
    {
        [MenuItem("GameObject/UI/SoundButton", false, 10)]
        static void CreateSoundButton(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("SoundButton");
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

            go.AddComponent<RectTransform>();
            go.AddComponent<Image>();
            go.AddComponent<SoundButton>();

            Undo.RegisterCreatedObjectUndo(go, "Create SoundButton");
            Selection.activeGameObject = go;
        }
    }
}