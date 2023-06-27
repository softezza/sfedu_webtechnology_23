using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AutotestingInspector
{
    public enum WindowAlertType
    {
        Deleting = 0,
        Warning
    }

    /// <summary>
    /// Логика взаимодействия для AlertWindow.xaml
    /// </summary>
    public partial class AlertWindow : Window
    {
        private AlertWindow()
        {
            InitializeComponent();
        }

        string _message;

        public AlertWindow(WindowAlertType type, string message = "Вы уверены что хотите удалить этот вариант?")
        {
            this.Top = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 3;
            this.Left = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 3;
            InitializeComponent();
            MessageBox.Text = message;
            _message = message;

            switch (type)
            {
                case WindowAlertType.Deleting:
                    Icon.Source = new BitmapImage(new Uri("Resources/Delete Icon.png", UriKind.Relative));
                    break;
                case WindowAlertType.Warning:
                    Icon.Source = new BitmapImage(new Uri("Resources/Warning.png", UriKind.Relative));
                    ButtonYES.Visibility = Visibility.Hidden;
                    ButtonNO.Visibility = Visibility.Hidden;
                    ButtonOK.Visibility = Visibility.Visible;
                    ButtonLogsLoad.Visibility = Visibility.Visible;

                    Loging(_message);
                    break;
            }
        }

        private void Loging(string errorMessage)
        {
            CashData.WriteLog(" Error: " + errorMessage);
        }

        private void ButtonLoadLogs(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "logs.txt";
            saveFileDialog.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                if (File.Exists(CashData._logAutotestingInspectorFile.FullName))
                {
                    File.Copy(CashData._logAutotestingInspectorFile.FullName, saveFileDialog.FileName, true);
                }
            }
        }

        public void ButtonOk(object sender, RoutedEventArgs e)
        {            
            Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void ButtonAccept(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ButtonNo(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
