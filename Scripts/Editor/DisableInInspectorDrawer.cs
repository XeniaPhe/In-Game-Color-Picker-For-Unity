using UnityEngine;
using UnityEditor;
using Xenia.ColorPicker.Utility;

namespace Xenia.ColorPicker.EditorScripts
{
    [CustomPropertyDrawer(typeof(DisableInInspector))]
    internal class DisableInInspectorDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            if (property.isExpanded)
            {
                height *= (property.CountInProperty() + 1);
            }

            return height;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}