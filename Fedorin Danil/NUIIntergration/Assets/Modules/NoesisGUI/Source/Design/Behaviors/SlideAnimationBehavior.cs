#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
using NoesisApp;
using Double = System.Single;
#else
using Double = System.Double;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
#endif
using System;

namespace SmartTwin.NoesisGUI.Behaviors
{
    /// <summary>
    /// Поведение слайд-анимации
    /// </summary>
    public class SlideAnimationBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty ShowProperty = DependencyProperty.Register(
           nameof(Show), typeof(bool), typeof(SlideAnimationBehavior), new PropertyMetadata(false, OnShowChanged));

        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(
          nameof(Direction), typeof(FlowDirection), typeof(SlideAnimationBehavior), new PropertyMetadata(FlowDirection.LeftToRight, OnShowChanged));

        private FrameworkElement _associatedObject;
        private Double _actualWidth;

        public bool Show
        {
            get { return (bool)GetValue(ShowProperty); }
            set { SetValue(ShowProperty, value); }
        }

        public FlowDirection Direction
        {
            get { return (FlowDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        private static void OnShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var slide = (SlideAnimationBehavior)d;
            slide.ShowAnimation(slide.Show);
        }

        private void ShowAnimation(bool isShow)
        {
            if (_associatedObject == null)
                return;

            DoubleAnimation animation = new DoubleAnimation
            {
                To = isShow ? 0 : Direction == FlowDirection.LeftToRight ? _actualWidth : -_actualWidth,
                Duration = TimeSpan.FromMilliseconds(250)
            };

            Storyboard.SetTarget(animation, _associatedObject);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);

            storyboard.Begin(_associatedObject);
        }

        protected override void OnAttached()
        {
            _associatedObject = AssociatedObject;
            _associatedObject.RenderTransform = new TranslateTransform();
            _associatedObject.SizeChanged += OnAssociatedObjectSizeChanged;
        }

        protected override void OnDetaching()
        {
            _associatedObject.SizeChanged -= OnAssociatedObjectSizeChanged;
            _associatedObject = null;
        }

        private void OnAssociatedObjectSizeChanged(object sender, SizeChangedEventArgs args)
        {
            if (args.WidthChanged)
            {
                _actualWidth = args.NewSize.Width;
                if (_associatedObject.RenderTransform is TranslateTransform translate)
                {
                    translate.X = Direction == FlowDirection.LeftToRight ? _actualWidth : -_actualWidth;
                }
            }
        }
    }
}
