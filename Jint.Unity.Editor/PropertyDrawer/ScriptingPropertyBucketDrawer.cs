using UnityEditor;
using UnityEngine;

namespace Jint.Unity.Editor
{
    [CustomPropertyDrawer(typeof(ScriptingPropertyBucket))]
    public class ScriptingPropertyBucketDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}