using System;
using UnityEngine;

namespace YSH.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class InspectorButtonAttribute : PropertyAttribute
    {
        public readonly string ButtonLabel;
        public readonly Mode ExecuteMode;

        public enum Mode
        {
            Always,     // 始终可见
            EditorOnly, // 仅在编辑器下显示
            PlayModeOnly // 仅在运行时显示
        }

        public InspectorButtonAttribute(string buttonLabel = null, Mode mode = Mode.Always)
        {
            this.ButtonLabel = buttonLabel;
            this.ExecuteMode = mode;
        }
    }
}