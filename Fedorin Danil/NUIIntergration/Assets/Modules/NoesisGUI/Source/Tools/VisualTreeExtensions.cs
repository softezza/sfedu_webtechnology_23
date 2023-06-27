#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
using NoesisApp;
#else
using System.Windows.Controls;
using System.Windows.Media;
#endif

using System;
using System.Collections.Generic;
using System.Windows;



namespace SmartTwin.NoesisGUI.Tools
{
    /// <summary>
    /// Класс расширений для визуального дерева
    /// </summary>
    public static class VisualTreeExtensions
    {
        /// <summary>
        /// Получить родителя элемента данного типа
        /// </summary>
        /// <typeparam name="T">Тип родителя</typeparam>
        /// <param name="obj"></param>
        /// <returns>Null, если родителя данного типа найти не удалось</returns>
        public static T GetParent<T>(this DependencyObject obj)
            where T : DependencyObject
        {
            if (obj == null)
                return null;

            var target = VisualTreeHelper.GetParent(obj);

            while (target != null)
            {
                if (target is T)
                    break;

                target = VisualTreeHelper.GetParent(target);
            }

            return target as T;
        }

        /// <summary>
        /// Получить дочерний элемент указанного типа
        /// </summary>
        /// <returns>Null, если дочерний элемент найти не удалось</returns>
        public static T GetChild<T>(this DependencyObject obj)
            where T : DependencyObject
        {
            if (obj == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                var result = (child as T) ?? GetChild<T>(child);
                if (result != null)
                    return result;
            }

            return null;
        }

		/// <summary>
		/// Получить дочерний элемент указанного типа
		/// </summary>
		/// <returns>Null, если дочерний элемент найти не удалось</returns>
		public static object GetChild(this DependencyObject obj, Type type)
        {
            if (obj == null)
                return null;

            if (!type.IsAssignableFrom(typeof(DependencyObject)))
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                if (child.GetType() == type)
                    return child;
                else
                {
                    child = child.GetChild(type) as DependencyObject;

                    if (child != null)
                        return child;
                }
            }

            return null;
        }

        /// <summary>
        /// Получить все дочерние элементы указанного типа
        /// </summary>
        /// <returns>Пустой лист, если дочерних элементов не нашлось</returns>
        public static List<T> GetChilds<T>(this DependencyObject obj)
            where T : DependencyObject
        {
			var result = new List<T>();

			if (obj == null)
                return result;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                if (child is T)
                    result.Add(child as T);

                result.AddRange(child.GetChilds<T>());
            }

            return result;
        }

		/// <summary>
		/// Получить все дочерние элементы указанного типа и удовлет удовлетворяющие требованиям
		/// </summary>
		/// <param name="predicate">Условие для отбора</param>
		/// <returns>Пустой лист, если дочерних элементов не нашлось</returns>
		public static List<T> GetChilds<T>(this DependencyObject obj, Predicate<T> predicate)
            where T : DependencyObject
        {
            if (obj == null)
                return null;

            var result = new List<T>();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                if (child is T element && predicate(element))
                    result.Add(element);

                result.AddRange(child.GetChilds(predicate));
            }

            return result;
        }

        [Obsolete]
        public static bool RemoveChild(this DependencyObject parent, UIElement child)
        {
#if NOESIS
            if (parent is Panel panel)
                return panel.Children.Remove(child);
#else
            if (parent is Panel panel)
            {
                if (panel.Children.Contains(child))
                {
                    panel.Children.Remove(child);
                    return true;
                }

                return false;
            }
#endif

            if (parent is Decorator decorator)
            {
                if (decorator.Child == child)
                {
                    decorator.Child = null;
                    return true;
                }

                return false;
            }

            if (parent is ContentPresenter presenter)
            {
                if (presenter.Content == child)
                {
                    presenter.Content = null;
                    return true;
                }

                return false;
            }

            if (parent is ContentControl control)
            {
                if (control.Content == child)
                {
                    control.Content = null;
                    return true;
                }

                return false;
            }

            return false;
        }

        [Obsolete]
        public static bool AddChild(this Panel panel, UIElement child)
        {
            if (panel.Children.Contains(child))
                return false;

            panel.Children.Add(child);

            return true;
        }

        [Obsolete]
        public static bool RemoveChild(this Panel panel, UIElement child)
        {
            if (!panel.Children.Contains(child))
                return false;

            panel.Children.Remove(child);

            return true;
        }

        /// <summary>
        /// Проверить, является ли элемент потомком другого элемента
        /// </summary>
        /// <param name="target">Проверяемый элемент</param>
        /// <param name="parent">Потенциальный родительский элемент</param>
        /// <returns></returns>
        public static bool IsDescendant(this DependencyObject target, DependencyObject parent)
        {
            foreach (var child in parent.GetChilds<DependencyObject>())
            {
                if (child == target)
                    return true;
            }

            return false;
        }

    }
}