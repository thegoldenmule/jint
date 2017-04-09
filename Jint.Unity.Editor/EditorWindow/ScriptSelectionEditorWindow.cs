using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Jint.Unity.Editor
{
    /// <summary>
    /// Basic editor window to look through all scripts and select one.
    /// </summary>
    public class ScriptSelectionEditorWindow : EditorWindow
    {
        /// <summary>
        /// Interface for something that wants to know what this EditorWindow
        /// is doing.
        /// </summary>
        public interface IScriptSelectionDelegate
        {
            /// <summary>
            /// Called when a selection is made in the window.
            /// </summary>
            /// <param name="asset"></param>
            void Selected(TextAsset asset);
        }

        /// <summary>
        /// Record of a specific script.
        /// </summary>
        public class ScriptSelectionRecord
        {
            /// <summary>
            /// Name of the script.
            /// </summary>
            public string Name;

            /// <summary>
            /// Absolute path to the script.
            /// </summary>
            public string AbsPath;

            /// <summary>
            /// Full text of the script. Lazily populated.
            /// </summary>
            public string Source;
        }

        /// <summary>
        /// Options for how scripts should be filtered.
        /// </summary>
        private enum FilterOption
        {
            FilePath,
            Contents
        }

        /// <summary>
        /// The skin to render with.
        /// </summary>
        private GUISkin _skin;

        /// <summary>
        /// ListComponent to render scripts with.
        /// </summary>
        private readonly ListComponent _scriptsListComponent = new ListComponent();

        /// <summary>
        /// All scripts.
        /// </summary>
        private ScriptSelectionRecord[] _scripts;

        /// <summary>
        /// Selected script.
        /// </summary>
        private ScriptSelectionRecord _selected;
        
        /// <summary>
        /// Holds the TextField value for the filter regex.
        /// </summary>
        private string _filterRegex;

        /// <summary>
        /// Compiles the filter when the filter changes.
        /// </summary>
        private Regex _filterRegexCompiled;

        /// <summary>
        /// Option for filtering.
        /// </summary>
        private FilterOption _filterOption;

        /// <summary>
        /// One thing can listen at a time.
        /// </summary>
        public IScriptSelectionDelegate Delegate;

        /// <summary>
        /// Sets selected script.
        /// </summary>
        public ScriptSelectionRecord Selected
        {
            get { return _selected; }
            set
            {
                if (_selected == value)
                {
                    return;
                }

                _selected = value;

                Repaint();
            }
        }

        /// <summary>
        /// Retrieves all scripts.
        /// </summary>
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

        /// <summary>
        /// Called by Unity when this EditorWindow is enabled.
        /// </summary>
        private void OnEnable()
        {
            FindAllScripts();

            titleContent = new GUIContent("Scripts");

            _filterOption = FilterOption.FilePath;
            _filterRegex = ".*";
            _filterRegexCompiled = new Regex(_filterRegex);

            _scriptsListComponent.OnSelected -= ScriptsList_OnSelected;
            _scriptsListComponent.OnSelected += ScriptsList_OnSelected;

            _scriptsListComponent.Populate(
                _scripts
                    .Select(script => new LabelListElement(
                            script,
                            script.AbsPath.Replace(Application.dataPath, "")))
                    .ToArray());
        }

        /// <summary>
        /// Called by Unity when this EditorWindow is disabled.
        /// </summary>
        private void OnDisable()
        {
            
        }

        /// <summary>
        /// Called by Unity when this EditorWindow is drawn.
        /// </summary>
        private void OnGUI()
        {
            // FOR TESTING
            Repaint();

            var skin = GUI.skin;
            if (null == _skin)
            {
                _skin = (GUISkin) EditorGUIUtility.Load("Jint.Dark.guiskin");
            }
            GUI.skin = _skin;

            GUILayout.BeginHorizontal(
                GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true));

            DrawScriptList();
            DrawScript();

            GUILayout.EndHorizontal();

            GUI.skin = skin;
        }

        /// <summary>
        /// Draws the list of scripts.
        /// </summary>
        private void DrawScriptList()
        {
            GUILayout.BeginVertical(
                GUILayout.ExpandHeight(true),
                GUILayout.Width(300f));

            DrawFilterControls();

            foreach (var element in _scriptsListComponent.Elements)
            {
                element.IsVisible = IsMatch(element.Value<ScriptSelectionRecord>());
            }
            _scriptsListComponent.Draw();

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Draws a specific script.
        /// </summary>
        private void DrawScript()
        {
            if (null == _selected)
            {
                //
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
                    GUILayout.Width(10000));
                GUI.enabled = enabled;
            }
        }

        /// <summary>
        /// Draws controls for the filter.
        /// </summary>
        private void DrawFilterControls()
        {
            GUILayout.BeginHorizontal();
            var regex = EditorGUILayout.TextField(_filterRegex, GUILayout.ExpandWidth(true));

            var width = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 60;
            _filterOption = (FilterOption) EditorGUILayout.EnumPopup(
                _filterOption,
                GUILayout.Width(80));
            EditorGUIUtility.labelWidth = width;
            GUILayout.EndHorizontal();

            if (regex != _filterRegex)
            {
                _filterRegex = regex;
                _filterRegexCompiled = new Regex(_filterRegex
                    .Replace("(", "\\(")
                    .Replace(")", "\\)")
                    .Replace("\\", "\\\\"));
            }
        }

        /// <summary>
        /// Called when a script has been selected.
        /// </summary>
        /// <param name="listElement"></param>
        private void ScriptsList_OnSelected(ListElement listElement)
        {
            Selected = _scripts.FirstOrDefault(
                script => script == listElement.Value<ScriptSelectionRecord>());
        }

        /// <summary>
        /// True if the script is matched by the filter.
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        private bool IsMatch(ScriptSelectionRecord script)
        {
            if (FilterOption.Contents == _filterOption)
            {
                if (string.IsNullOrEmpty(script.Source))
                {
                    script.Source = File.ReadAllText(script.AbsPath);
                }

                return _filterRegexCompiled.IsMatch(script.Source);
            }

            return _filterRegexCompiled.IsMatch(script.AbsPath);
        }
    }
}