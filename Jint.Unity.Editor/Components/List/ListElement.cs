using System;

namespace Jint.Unity.Editor
{
    /// <summary>
    /// Basic building block of the list.
    /// </summary>
    public abstract class ListElement
    {
        /// <summary>
        /// Called when a repaint is requested.
        /// </summary>
        public event Action<ListElement> OnRepaint;

        /// <summary>
        /// Called when the ListElement has been selected.
        /// </summary>
        public event Action<ListElement> OnSelected;

        /// <summary>
        /// True if this element has been selected.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// True if the element is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// True if the element should be rendered at all.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Retrieves a typed value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract T Value<T>();

        /// <summary>
        /// Draws the element's controls.
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ListElement()
        {
            IsEnabled = true;
        }

        /// <summary>
        /// For base classes to call the OnSelected event.
        /// </summary>
        /// <param name="element"></param>
        protected void Selected(ListElement element)
        {
            if (null != OnSelected)
            {
                OnSelected(element);
            }
        }
    }
}