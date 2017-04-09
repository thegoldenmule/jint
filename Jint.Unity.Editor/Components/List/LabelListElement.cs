using UnityEngine;

namespace Jint.Unity.Editor
{
    /// <summary>
    /// Basic ListElement implementation that just has a label + a value.
    /// </summary>
    public class LabelListElement : ListElement
    {
        /// <summary>
        /// The label.
        /// </summary>
        private readonly string _label;

        /// <summary>
        /// The value.
        /// </summary>
        private readonly object _value;

        /// <summary>
        /// Casts the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override T Value<T>()
        {
            return (T) _value;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="label"></param>
        public LabelListElement(
            object value,
            string label)
        {
            _value = value;
            _label = label;
        }

        /// <summary>
        /// Draws the element.
        /// </summary>
        public override void Draw()
        {
            GUILayout.Label(
                new GUIContent(_label),
                IsEnabled
                    ? (IsSelected ? "listitem-selected" : "listitem-unselected")
                    : "listitem-disabled",
                GUILayout.ExpandWidth(true));

            var rect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.mouseUp
                && rect.Contains(Event.current.mousePosition))
            {
                Selected(this);
            }
        }
    }
}