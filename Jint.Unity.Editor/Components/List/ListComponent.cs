using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jint.Unity.Editor
{
    /// <summary>
    /// Basic list component.
    /// </summary>
    public class ListComponent
    {
        /// <summary>
        /// All elements in the list.
        /// </summary>
        private readonly List<ListElement> _elements = new List<ListElement>();

        /// <summary>
        /// Scroll position.
        /// </summary>
        private Vector2 _position;

        /// <summary>
        /// Backing variable for property.
        /// </summary>
        private ListElement _selected;

        /// <summary>
        /// Copy of elements.
        /// </summary>
        public ListElement[] Elements
        {
            get { return _elements.ToArray();  }
        }

        /// <summary>
        /// Currently selected element.
        /// </summary>
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

                foreach (var element in _elements)
                {
                    if (element != _selected)
                    {
                        element.IsSelected = false;
                    }
                }

                if (null != _selected)
                {
                    _selected.IsSelected = true;
                }

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

        /// <summary>
        /// Called when the component needs repainted.
        /// </summary>
        public event Action OnRepaint;

        /// <summary>
        /// Called when an element has been selected.
        /// </summary>
        public event Action<ListElement> OnSelected;

        /// <summary>
        /// Populates the list with elements.
        /// </summary>
        /// <param name="elements"></param>
        public void Populate(ListElement[] elements)
        {
            _selected = null;

            foreach (var element in _elements)
            {
                element.IsSelected = false;
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

            if (_elements.Count > 0)
            {
                Selected = _elements[0];
            }
        }

        /// <summary>
        /// Draws the component.
        /// </summary>
        public void Draw()
        {
            _position = GUILayout.BeginScrollView(
                _position,
                GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true));

            foreach (var element in _elements)
            {
                if (element.IsVisible)
                {
                    element.Draw();
                }
            }

            GUILayout.EndScrollView();

            if (Event.current.type == EventType.KeyUp)
            {
                var index = _elements.IndexOf(_selected);
                if (-1 != index)
                {
                    if (Event.current.keyCode == KeyCode.UpArrow)
                    {
                        while (index > 0)
                        {
                            index--;

                            if (_elements[index].IsEnabled)
                            {
                                Selected = _elements[index];
                                break;
                            }
                        }
                    }
                    else if (Event.current.keyCode == KeyCode.DownArrow)
                    {
                        while (index < _elements.Count - 1)
                        {
                            index++;

                            if (_elements[index].IsEnabled)
                            {
                                Selected = _elements[index];
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called when an element fires an OnSelected event.
        /// </summary>
        /// <param name="element"></param>
        private void Element_OnSelected(ListElement element)
        {
            Selected = element;
        }

        /// <summary>
        /// Called when an element fires a repaint event.
        /// </summary>
        /// <param name="element"></param>
        private void Element_OnRepaint(ListElement element)
        {
            if (null != OnRepaint)
            {
                OnRepaint();
            }
        }
    }
}