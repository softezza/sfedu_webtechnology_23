#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
using NoesisApp;
#else
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
#endif
using SmartTwin.NoesisGUI.Tools;

namespace SmartTwin.NoesisGUI.Behaviors
{
    /// <summary>
    /// Поведение вложенного скрола. 
    /// ToDo: Семену на доописание
    /// </summary>
    public class NestedScrollBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
                AssociatedObject.PreviewMouseWheel += PreviewMouseWheel;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
                AssociatedObject.PreviewMouseWheel -= PreviewMouseWheel;
            base.OnDetaching();
        }

        private void PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = AssociatedObject.GetChild<ScrollViewer>();
            if (scrollViewer != null)
            {
                var scrollPos = scrollViewer.VerticalOffset;
                if ((scrollPos == scrollViewer.ScrollableHeight && e.Delta < 0)
                    || (scrollPos == 0 && e.Delta > 0))
                {
                    e.Handled = true;
#if NOESIS
                    var e2 = new MouseWheelEventArgs(AssociatedObject, ScrollViewer.MouseWheelEvent, e.Delta);
#else
                    var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                    e2.RoutedEvent = UIElement.MouseWheelEvent;
#endif
                    AssociatedObject.RaiseEvent(e2);
                }
            }
        }
    }
}
