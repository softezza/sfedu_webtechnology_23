#if UNITY_5_3_OR_NEWER
#define NOESIS
    using Noesis;
#else
using System.Windows;
#endif

namespace SmartTwin.NoesisGUI.AttachedProperties
{
    /// <summary>
    /// Класс прикрепляемых свойств для связки Visible свойства с bool флагом.
    /// ToDo: на доработку
    /// </summary>
    public class VisibleMode
    {
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.RegisterAttached("IsVisible", typeof(bool), typeof(VisibleMode),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertyChanged));

        public static readonly DependencyProperty EnabledCollapsProperty =
            DependencyProperty.RegisterAttached("EnabledCollaps", typeof(bool), typeof(VisibleMode),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertyChanged));

        public static readonly DependencyProperty InverseProperty =
           DependencyProperty.RegisterAttached("Inverse", typeof(bool), typeof(VisibleMode),
           new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender, OnPropertyChanged));

        public static bool GetIsVisible(UIElement target) =>
            (bool)target.GetValue(IsVisibleProperty);

        public static void SetIsVisible(UIElement target, bool value) =>
            target.SetValue(IsVisibleProperty, value);

        public static bool GetEnabledCollaps(UIElement target) =>
           (bool)target.GetValue(EnabledCollapsProperty);

        public static void SetEnabledCollaps(UIElement target, bool value) =>
            target.SetValue(EnabledCollapsProperty, value);

        public static bool GetInverse(UIElement target) =>
          (bool)target.GetValue(InverseProperty);

        public static void SetInverse(UIElement target, bool value) =>
            target.SetValue(InverseProperty, value);

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as UIElement;
            if (element == null)
                return;

            var value = (bool)e.NewValue;

            if (e.Property == IsVisibleProperty)
            {
                var inverse = GetInverse(element);
                if (value)
                    element.Visibility = inverse ? (GetEnabledCollaps(element) ? Visibility.Collapsed : Visibility.Hidden) : Visibility.Visible;
                else
                    element.Visibility = inverse ? Visibility.Visible : (GetEnabledCollaps(element) ? Visibility.Collapsed : Visibility.Hidden);
            }

            if (e.Property == EnabledCollapsProperty)
            {
                if (!GetIsVisible(element))
                    element.Visibility = value ? Visibility.Collapsed : Visibility.Hidden;
            }

            if (e.Property == InverseProperty)
            {
                var visible = GetIsVisible(element);
                var collapse = GetEnabledCollaps(element);
                if (visible)
                    element.Visibility = collapse ? Visibility.Collapsed : Visibility.Hidden;
                else
                    element.Visibility = Visibility.Visible;
                    
            }
        }
    }
}