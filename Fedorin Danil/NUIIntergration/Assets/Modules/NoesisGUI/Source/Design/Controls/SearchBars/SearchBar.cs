#if UNITY_5_3_OR_NEWER
#define NOESIS
    using Noesis;
#else
using System.Windows;
using System.Windows.Controls;
#endif
using SmartTwin.NoesisGUI.Commands;
using System;

namespace SmartTwin.NoesisGUI.Controls
{
    /// <summary>
    /// Текстовый поиск
    /// </summary>
    public class SearchBar : TextBox
    {
        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
            nameof(PlaceholderText), typeof(string), typeof(SearchBar), new PropertyMetadata(null));

        public static readonly DependencyProperty TextChangedCommandProperty = DependencyProperty.Register(
            nameof(TextChangedCommand), typeof(DelegateCommand), typeof(SearchBar), new PropertyMetadata(null));

        public static readonly DependencyProperty FocusedCommandProperty = DependencyProperty.Register(
            nameof(FocusedCommand), typeof(DelegateCommand), typeof(SearchBar), new PropertyMetadata(null));

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            nameof(IsBusy), typeof(bool), typeof(SearchBar), new PropertyMetadata(false));

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public DelegateCommand TextChangedCommand
        {
            get { return (DelegateCommand)GetValue(TextChangedCommandProperty); }
            set { SetValue(TextChangedCommandProperty, value); }
        }

        public DelegateCommand FocusedCommand
        {
            get { return (DelegateCommand)GetValue(FocusedCommandProperty); }
            set { SetValue(FocusedCommandProperty, value); }
        }

        public DelegateCommand ClearTextCommand { get; }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public SearchBar()
        {
            var weak = new WeakReference(this);
            TextChanged += (o, e) => ((SearchBar)weak.Target).TextChangedCommand?.Execute(Text);
            GotFocus += (o, e) => ((SearchBar)weak.Target).FocusedCommand?.Execute(true);
            LostFocus += (o, e) => ((SearchBar)weak.Target).FocusedCommand?.Execute(false);

            ClearTextCommand = new DelegateCommand(OnClearText);
        }

        private void OnClearText(object obj)
        {
            Text = "";
            Focus();
        }
    }
}
