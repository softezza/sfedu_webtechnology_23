#if UNITY_5_3_OR_NEWER
#define NOESIS
using Noesis;
#else
using System.Windows.Controls;
using System.Windows;
#endif

using System;

namespace SmartTwin.NoesisGUI.Regions
{
    using System.Collections.ObjectModel;
    using Views;

    /// <summary>
    /// Поведение региона для отображения конкретного представления из коллекции
    /// </summary>
    public class ViewPresenterRegionBehavior : RegionBehavior<ItemsControl>
    {
        public static readonly DependencyProperty CurrentIndexProperty = DependencyProperty.Register(
                nameof(CurrentIndex),
                typeof(int),
                typeof(ViewPresenterRegionBehavior),
                new PropertyMetadata(0, OnCurrentChanged));

        public static readonly DependencyProperty CurrentItemProperty = DependencyProperty.Register(
               nameof(CurrentItem),
               typeof(BaseView),
               typeof(ViewPresenterRegionBehavior),
               new PropertyMetadata(null));

        /// <summary>
        /// Текущий активный индекс представления
        /// </summary>
        public int CurrentIndex
        {
            get => (int)GetValue(CurrentIndexProperty);
            set => SetValue(CurrentIndexProperty, value);
        }

        /// <summary>
        /// Текущее активное представление
        /// </summary>
        public BaseView CurrentItem
        {
            get => (BaseView)GetValue(CurrentItemProperty);
            set => SetValue(CurrentItemProperty, value);
        }

        /// <summary>
        /// Представления
        /// ToDo: бесмысленно, т.к. есть представления в регионах
        /// </summary>
        public ObservableCollection<BaseView> Items { get; }

        public ViewPresenterRegionBehavior()
        {
            Items = new ObservableCollection<BaseView>();
        }

        /// <summary>
        /// Вызывается при изменении свойства <see cref="CurrentIndex"/>
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnCurrentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var b = (ViewPresenterRegionBehavior)d;
            if (e.NewValue is int selectedIndex)
            {
                for (int i = 0; i < b.Items.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        b.CurrentItem = b.Items[i];
                        b.Items[i].Visibility = Visibility.Visible;
                        b.Items[i].IsActive = true;
                    }
                    else
                    {
                        b.Items[i].Visibility = Visibility.Hidden;
                        b.Items[i].IsActive = false;
                    }
                }
            }
        }

        public override bool TryAdd(BaseView view)
        {
            var container = Container;

            if (container == null)
                return false;

            var children = container.Items;

            if (children.Contains(view))
                return false;

            Items.Add(view);
            children.Add(view);

            if (Items.Count - 1 == CurrentIndex)
            {
                CurrentItem = view;
                view.Visibility = Visibility.Visible;
                view.IsActive = true;
            }
            else
            {
                view.Visibility = Visibility.Hidden;
                view.IsActive = false;  
            }

            return true;
        }

        public override bool TryRemove(BaseView view)
        {
            var container = Container;

            if (container == null)
                return false;

            var children = container.Items;

            if (!children.Contains(view))
                return false;

            Items.Remove(view);
            children.Remove(view);
            return true;
        }


        public override bool TryClear()
        {
            var container = Container;

            if (container == null)
                return false;

            var children = container.Items;

            children.Clear();

            return true;
        }

        protected override Exception ValidateContainer(ItemsControl container)
        {
            if (container.Items.Count > 0)
                return new InvalidOperationException("Invalid Region - Panel is not Empty");

            return null;
        }
    }
}
