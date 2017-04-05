using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jint.Unity.Editor
{
    public class LabelListElement : ListElement
    {
        private readonly string _label;

        public readonly object Value;

        public LabelListElement(
            object value,
            string label)
        {
            Value = value;
            _label = label;
        }

        public override void Draw()
        {
            base.Draw();

            GUILayout.Label(new GUIContent(_label), GUILayout.ExpandWidth(true));
            var rect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.mouseUp
                && rect.Contains(Event.current.mousePosition))
            {
                Selected(this);
            }
        }
    }

    public class ListElement
    {
        public event Action<ListElement> OnRepaint;
        public event Action<ListElement> OnSelected;

        public virtual void Draw()
        {
            //
        }

        protected void Selected(ListElement element)
        {
            if (null != OnSelected)
            {
                OnSelected(element);
            }
        }
    }

    public class ListComponent
    {
        private readonly List<ListElement> _elements = new List<ListElement>();
        private Vector2 _position;
        private ListElement _selected;

        public ListElement Selected
        {
            get
            {
                return _selected;
            }

            set
            {
                if (_selected == value)
                {
                    return;
                }

                _selected = value;

                if (null != OnSelected)
                {
                    OnSelected(_selected);
                }

                if (null != OnRepaint)
                {
                    OnRepaint();
                }
            }
        }

        public event Action OnRepaint;
        public event Action<ListElement> OnSelected;

        public void Populate(ListElement[] elements)
        {
            foreach (var element in _elements)
            {
                element.OnRepaint -= Element_OnRepaint;
                element.OnSelected -= Element_OnSelected;
            }
            _elements.Clear();

            foreach (var element in elements)
            {
                element.OnRepaint += Element_OnRepaint;
                element.OnSelected += Element_OnSelected;
                _elements.Add(element);
            }
        }

        public void Draw()
        {
            _position = GUILayout.BeginScrollView(
                _position,
                GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true));

            foreach (var element in _elements)
            {
                element.Draw();
            }

            GUILayout.EndScrollView();
        }

        private void Element_OnSelected(ListElement element)
        {
            Selected = element;
        }

        private void Element_OnRepaint(ListElement element)
        {
            if (null != OnRepaint)
            {
                OnRepaint();
            }
        }
    }
}