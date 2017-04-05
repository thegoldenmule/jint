using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Jint.Unity.Editor
{
    public class ScriptSelectionEditorWindow : EditorWindow
    {
        public interface IScriptSelectionDelegate
        {
            void Selected(TextAsset asset);
        }

        private class ScriptSelectionRecord
        {
            public string Name;
            public string AbsPath;
            public string Source;
        }

        private readonly ListComponent _scriptsListComponent = new ListComponent();
        private ScriptSelectionRecord[] _scripts;
        private ScriptSelectionRecord _selected;
        private Vector2 _scriptListPosition;
        private Vector2 _scriptPreviewPosition;

        public IScriptSelectionDelegate Delegate;

        private void FindAllScripts()
        {
            _scripts = Directory
                .GetFiles(
                    Application.dataPath,
                    "*.html",
                    SearchOption.AllDirectories)
                .Select(file => new ScriptSelectionRecord
                {
                    Name = Path.GetFileName(file),
                    AbsPath = file
                })
                .ToArray();
            _selected = _scripts.FirstOrDefault();
        }

        private void OnEnable()
        {
            FindAllScripts();

            titleContent = new GUIContent("Scripts");

            _scriptsListComponent.Populate(
                _scripts
                    .Select(script => new LabelListElement(script, script.AbsPath))
                    .ToArray());
        }

        private void OnDisable()
        {
            
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal(
                GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true));

            DrawScriptList();
            DrawScript();

            GUILayout.EndHorizontal();
        }

        private void DrawScriptList()
        {
            GUILayout.BeginVertical(
                GUILayout.ExpandHeight(true),
                GUILayout.Width(Screen.width/2f));

            _scriptsListComponent.Draw();

            GUILayout.EndVertical();
        }

        private void DrawScript()
        {
            _scriptPreviewPosition = GUILayout.BeginScrollView(
                _scriptPreviewPosition,
                GUILayout.ExpandHeight(true),
                GUILayout.Width(Screen.width / 2f));

            if (null == _selected)
            {

            }
            else
            {
                if (string.IsNullOrEmpty(_selected.Source))
                {
                    _selected.Source = File.ReadAllText(_selected.AbsPath);
                }

                var enabled = GUI.enabled;
                GUI.enabled = false;
                GUILayout.TextArea(
                    _selected.Source,
                    GUILayout.ExpandHeight(true),
                    GUILayout.ExpandWidth(true));
                GUI.enabled = enabled;
            }

            GUILayout.EndScrollView();
        }
    }
}