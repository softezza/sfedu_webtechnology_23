#if UNITY_5_3_OR_NEWER
using Noesis;
using System;
using UnityEngine;

namespace SmartTwin.NoesisGUI.Physics
{
    /// <summary>
    /// Система взимодействия Unity с Noesis
    /// </summary>
    public class PhysicsNoesisSystem
	{
        /// <summary>
        /// Главный экран
        /// </summary>
        private MainScreen _screen;

        /// <summary>
        /// Контент главного экрана
        /// </summary>
        private FrameworkElement _content;

        /// <summary>
        /// Данные о мышке
        /// </summary>
        private Mouse _mouse;


        /// <summary>
        /// Флаг инициализации
        /// </summary>
        private bool _isInitialized = false;


        /// <summary>
        /// Инициализировать систему
        /// </summary>
        /// <param name="screen">Главный экран</param>
        /// <exception cref="NullReferenceException">Если <see cref="MainScreen"/> null</exception>
        /// <exception cref="InvalidCastException">Если <see cref="MainScreen.Content"/> не является <see cref="FrameworkElement"/></exception>
        public void Initialize(MainScreen screen)
        {
            _screen = screen;
            _mouse = screen.Mouse;

            if (_screen == null)
                throw new NullReferenceException("MainScreen is NULL!");

            if (_screen.Content is FrameworkElement content)
				_content = content;
			else 
                throw new InvalidCastException("MainScreen Content is not cast to FrameworkElement!");

			_isInitialized = true;
        }


        /// <summary>
        /// Находится ли мышка поверх интерфеса
        /// </summary>
        /// <returns>True, если находится</returns>
		public bool IsMouseOver()
		{
            if (!_isInitialized)
                return false;

            if (_mouse.Captured != null)
                return true;

			return IsMouseOverInternal();
		}

        /// <summary>
        /// Захватить фокус. Если фокус захвачен, то IsMouseOver всегда будет возвращать True
        /// </summary>
        public void CaptureInteractiveFocus() => _content.CaptureMouse();

        /// <summary>
        /// Освободить фокус. 
        /// </summary>
        public void ReleaseInteractiveFocus() => _content.ReleaseMouseCapture();


        /// <summary>
        /// Внутренняя реализация <see cref="IsMouseOver"/>
        /// </summary>
        /// <returns></returns>
		private bool IsMouseOverInternal()
        {
            if (!_isInitialized) 
                return false;

            var mousePosition = Input.mousePosition;

            var point = _content.PointFromScreen(new Point(mousePosition.x, Screen.height - mousePosition.y));

            DependencyObject visualHit = null;
            VisualTreeHelper.HitTest(_content,
                FilterHits,
                hit => ProcessHitResult(hit, out visualHit),
                new PointHitTestParameters(point));

            if (visualHit != null)
                return true;

            return false;
        }

        /// <summary>
        /// Фильтрует HitTest по визуальному дереву
        /// </summary>
        /// <param name="target">Текущий элемент в дереве</param>
        /// <returns>Возвращает инструкцию, что как проходить дерево дальше</returns>
        private HitTestFilterBehavior FilterHits(Visual target)
        {
            if (target is FrameworkElement element)
            {
                if (!element.IsHitTestVisible)
                    return HitTestFilterBehavior.ContinueSkipSelfAndChildren;
            }

            return HitTestFilterBehavior.Continue;
        }

        /// <summary>
        /// Обрабатывать HitTest
        /// </summary>
        /// <param name="hit">Пересечение с элементами</param>
        /// <param name="visualHit">Найденный элемент, который должен вернуться в HitTest</param>
        /// <returns>Возвращает инструкцию, как обработка должна идти дальше</returns>
        private HitTestResultBehavior ProcessHitResult(HitTestResult hit, out DependencyObject visualHit)
        {
            visualHit = null;

            if (hit.VisualHit is FrameworkElement element)
            {
                if (element.IsHitTestVisible)
                {
                    visualHit = element;

                    return HitTestResultBehavior.Stop;
                }
                else
                {
                    return HitTestResultBehavior.Continue;
                }
            }

            return HitTestResultBehavior.Stop;
        }
    }
}
#endif