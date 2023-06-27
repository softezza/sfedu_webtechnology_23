#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
using NoesisApp;
#else
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
#endif

using System;
using System.Linq;
using SmartTwin.NoesisGUI.Commands;
using SmartTwin.NoesisGUI.Tools;

namespace SmartTwin.NoesisGUI.Behaviors
{
    /// <summary>
    /// Поведение перемещения элемента мышкой
    /// </summary>
    public class DraggableBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty IsDraggingProperty;

        public static readonly DependencyProperty DragTargetProperty;

        public static readonly DependencyProperty ConstrainToParentBoundsProperty;

        public static readonly DependencyProperty XProperty;

        public static readonly DependencyProperty YProperty;

        /// <summary>
        /// Трансформ перемещения
        /// </summary>
        private TranslateTransform _translate;

		/// <summary>
		/// Элемент, нажатие на который нужно отслеживать для перемещения.
		/// Цель может быть как основным элементом, к которому прикреплено поведение, так и другим элементов (зачастую дочерним).
		/// </summary>
		private WeakReference<FrameworkElement> _dragTarget;

        /// <summary>
        /// Позиция элемента
        /// </summary>
        private Point pointPosition;

        /// <summary>
        /// Флаг, отвечающий за то, должно ли перемещение ограничивается рамками родительского контейнерами или нет
        /// </summary>
        private bool constrainToParentBounds;


        static DraggableBehavior()
        {
            IsDraggingProperty = DependencyProperty.Register(nameof(IsDragging), typeof(bool), typeof(DraggableBehavior), new PropertyMetadata(true));

            DragTargetProperty = DependencyProperty.Register(nameof(DragTarget), typeof(FrameworkElement), typeof(DraggableBehavior), new PropertyMetadata(null));

            ConstrainToParentBoundsProperty = DependencyProperty.Register(
                nameof(ConstrainToParentBounds),
                typeof(bool),
                typeof(DraggableBehavior),
                new PropertyMetadata(true, OnConstaintToParentBoundsChanged));

            XProperty = DependencyProperty.Register(nameof(X), typeof(float), typeof(DraggableBehavior), new PropertyMetadata(0f));

            YProperty = DependencyProperty.Register(nameof(Y), typeof(float), typeof(DraggableBehavior), new PropertyMetadata(0f));

        }

        /// <summary>
        /// Вызывается при изменении свойства <see cref="ConstrainToParentBounds"/>
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnConstaintToParentBoundsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (DraggableBehavior)d;
            behavior.constrainToParentBounds = (bool)e.NewValue;
        }

        /// <summary>
        /// Свойство, отвечающее за то, в состоянии перемещения сейчас элемент или нет
        /// </summary>
        public bool IsDragging
        {
            get => (bool)GetValue(IsDraggingProperty);
            set => SetValue(IsDraggingProperty, value);
        }

		/// <summary>
		/// Элемент, нажатие на который нужно отслеживать для перемещения.
		/// Цель может быть как основным элементом, к которому прикреплено поведение, так и другим элементов (зачастую дочерним).
		/// </summary>
		public FrameworkElement DragTarget
        {
            get => (FrameworkElement)GetValue(DragTargetProperty);
            set => SetValue(DragTargetProperty, value);
        }

		/// <summary>
		/// Флаг, отвечающий за то, должно ли перемещение ограничивается рамками родительского контейнерами или нет
		/// </summary>
		public bool ConstrainToParentBounds
        {
            get => (bool)GetValue(ConstrainToParentBoundsProperty);
            set => SetValue(ConstrainToParentBoundsProperty, value);
        }

        /// <summary>
        /// Позиция элемента в X-координате
        /// </summary>
        public float X
        {
            get => (float)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        /// <summary>
        /// Позиция элемента в Y-координате
        /// </summary>
        public float Y
        {
            get => (float)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }


        /// <summary>
        /// Вызывается когда поведение прикрепляется к элементу
        /// </summary>
        /// <exception cref="Exception">Если элемент не имеет TranslateTransform</exception>
        protected override void OnAttached()
        {
            var element = AssociatedObject;

            _translate = GetTranslateTransform(element);

            if (_translate == null)
                throw new Exception("Not found translate transform!");


            if (DragTarget == null)
                _dragTarget = new WeakReference<FrameworkElement>(element);
            else
                _dragTarget = new WeakReference<FrameworkElement>(DragTarget);

            constrainToParentBounds = ConstrainToParentBounds;

            if (_dragTarget.TryGetTarget(out var target))
                target.MouseLeftButtonDown += OnDragTargetMouseDown;

            if (IsDragging)
            {
                element.ExecuteWhenLoad(_ => SetPosition(X, Y));
            }
        }

        /// <summary>
        /// Вызывается, когда поведение открепляется от элемента
        /// </summary>
        protected override void OnDetaching()
        {
            var element = AssociatedObject;

            if (element == null)
                return;

            _translate = null;

            if (_dragTarget != null && _dragTarget.TryGetTarget(out var target))
            {
                target.MouseLeftButtonDown -= OnDragTargetMouseDown;
                _dragTarget = null;
            }
        }


        /// <summary>
        /// Получить <see cref="TranslateTransform"/>
        /// </summary>
        /// <param name="element">Элемент у которого нужно найти <see cref="TranslateTransform"/></param>
        /// <returns><see cref="TranslateTransform"/>. Может хранится как индивидуально, так и в <see cref="TransformGroup"/>.</returns>
        private TranslateTransform GetTranslateTransform(FrameworkElement element)
        {
            var transform = element.RenderTransform;

            if (transform is TranslateTransform translate)
                return translate;

            if (transform is TransformGroup group)
                return group.Children.FirstOrDefault(t => t is TranslateTransform) as TranslateTransform;

            return null;
        }


		/// <summary>
		/// Вызывается, когда срабатывает событие <see cref="MouseLeftButtonDown"/> 
		/// </summary>
		/// <param name="sender">Источник события</param>
		/// <param name="args">Аргументы события</param>
		private void OnDragTargetMouseDown(object sender, MouseButtonEventArgs args)
        {
            var target = (FrameworkElement)sender;

            pointPosition = args.GetPosition(target);

            target.MouseMove += OnMouseMove;
            target.LostMouseCapture += OnLostMouseCapture;
            target.MouseLeftButtonUp += OnMouseLeftButtonUp;

            target.CaptureMouse();

            args.Handled = true;
        }

        /// <summary>
        /// Вызывается, когда срабатывает событие MouseMove
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="args">Аргументы события</param>
        private void OnMouseMove(object sender, MouseEventArgs args)
        {
            var target = (FrameworkElement)sender;

            var newPosition = args.GetPosition(target);

            var offset = newPosition - pointPosition;

#if NOESIS
            UpdatePosition(offset.X, offset.Y);
#endif
            args.Handled = true;
        }

        /// <summary>
        /// Вызывается, когда срабатывает событие MouseLeftButtonUp
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="args">Аргументы события</param>
        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs args)
        {
            var target = (FrameworkElement)sender;

            target.ReleaseMouseCapture();

            args.Handled = true;
        }

		/// <summary>
		/// Вызывается при событии LostMouseCapture
		/// </summary>
		/// <param name="sender">Источник события</param>
		/// <param name="args">Аргументы события</param>
		private void OnLostMouseCapture(object sender, MouseEventArgs args)
        {
            FrameworkElement target = (FrameworkElement)sender;

            target.MouseMove -= OnMouseMove;
            target.LostMouseCapture -= OnLostMouseCapture;
            target.MouseLeftButtonUp -= OnMouseLeftButtonUp;

            args.Handled = true;
        }

        /// <summary>
        /// Установить новую позицию элементу
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        private void SetPosition(float x, float y)
        {
            var xOffset = x - _translate.X;
            var yOffset = y - _translate.Y;
            UpdatePosition(xOffset, yOffset);
        }

#if NOESIS
		/// <summary>
		/// Обновить позицию (работает только в контексте Noesis)
		/// </summary>
		/// <param name="x">Координата X</param>
		/// <param name="y">Координата Y</param>
		private void UpdatePosition(float x, float y)
        {
            var target = AssociatedObject;
            var parent = target.Parent;

            if (parent == null)
                return;

            if (constrainToParentBounds)
            {
                //Надо будет отдельно изучить вопрос с геометрией в ноезис
                Point minXY = new Point(0.0f, 0.0f);
                Point maxXY = new Point(parent.ActualWidth, parent.ActualHeight);

                Matrix4 m = target.TransformToAncestor(parent);
                Vector v0 = new Vector(m[3][0] + x, m[3][1] + y);
                Vector v1 = new Vector(target.ActualWidth, target.ActualHeight);
                v1 += v0;

                if (v1.X > maxXY.X)
                {
                    float dif = v1.X - maxXY.X;
                    x -= dif;
                    v0.X -= dif;
                    v1.X -= dif;
                }
                if (v0.X < minXY.X)
                {
                    float dif = minXY.X - v0.X;
                    x += dif;
                }
                if (v1.Y > maxXY.Y)
                {
                    float dif = v1.Y - maxXY.Y;
                    y -= dif;
                    v0.Y -= dif;
                    v1.Y -= dif;
                }
                if (v0.Y < minXY.Y)
                {
                    float dif = minXY.Y - v0.Y;
                    y += dif;
                }
            }

            _translate.X += x;
            _translate.Y += y;

            X = _translate.X;
            Y = _translate.Y;
        }
#else
        /// <summary>
		/// Обновить позицию (заглушка для контекста WPF)
		/// </summary>
		/// <param name="x">Координата X</param>
		/// <param name="y">Координата Y</param>
		public void UpdatePosition(double x, double y)
        {

        }
#endif
	}
}
