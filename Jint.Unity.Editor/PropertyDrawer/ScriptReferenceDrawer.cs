using UnityEditor;
using UnityEngine;

namespace Jint.Unity.Editor
{
    [CustomPropertyDrawer(typeof(ScriptReference))]
    public class ScriptReferenceDrawer : PropertyDrawer, ScriptSelectionEditorWindow.IScriptSelectionDelegate
    {
        private SerializedProperty _assetProperty;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 200;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            _assetProperty = property.FindPropertyRelative("Asset");
            var asset = (TextAsset) _assetProperty.objectReferenceValue;

            var textHeight = 20;
            var buttonSize = 20;

            var enabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.TextField(
                new Rect(position.x, position.y, position.width - buttonSize, textHeight),
                null == asset
                    ? "None (Javascript)"
                    : AssetDatabase.GetAssetPath(asset));
            GUI.enabled = enabled;

            if (GUI.Button(
                new Rect(position.x + position.width - buttonSize, position.y, buttonSize, buttonSize),
                "+"))
            {
                EditorWindow
                    .GetWindow<ScriptSelectionEditorWindow>()
                    .Delegate = this;
            }
        }

        public void Selected(TextAsset asset)
        {
            _assetProperty.objectReferenceValue = asset;
        }
    }
}