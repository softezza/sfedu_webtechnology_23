using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AutotestingInspectorSystem;

namespace AutotestingInspector
{
    /// <summary>
    /// Логика взаимодействия для LabCheackWindow.xaml
    /// </summary>
    public partial class LabInitWindow : Window
    {
        private double _topPos = 0;
        private double _leftPos = 0;

        private Editor _editor;

        private string _pathLab = string.Empty;

        private LabInitWindow()
        {
            InitializeComponent();
        }

        public LabInitWindow(double topPos, double leftPos)
        {
            this.Top = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 3;
            this.Left = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 3;
            InitializeComponent();
            _topPos = topPos;
            _leftPos = leftPos;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void ButtonLabDerictory(object sender, RoutedEventArgs e)
        {
            DirectoryPathChoice();
        }

        private void ButtonBack(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ButtonSave(object sender, RoutedEventArgs e)
        {
            Saving();
        }

        private async void Saving()
        {
            if (NameLabTextBox.Text != string.Empty && DirectoryBox.Text != string.Empty)
            {                   
                await CreateLabWork(NameLabTextBox.Text, _pathLab);
            }
            else
            {
                CashData.Alert("Не все поля заполнены.");
            }            
        }

        private void DirectoryPathChoice()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _pathLab = dialog.SelectedPath;
            }

            DirectoryBox.Text = _pathLab;
        }

        private async Task CreateLabWork(string name, string path)
        {
            try
            {
                var editor = await CashData.Inspector.CreateLabWork(name, path);
                _editor = editor;

                AddVariantsWindow addLabWindow = new AddVariantsWindow(_editor);
                addLabWindow.Top = _topPos;
                addLabWindow.Left = _leftPos;
                addLabWindow.Show();
                DialogResult = true;
                Close();
            }
            catch(Exception ex)
            {
                CashData.Alert(ex.Message);
            }  
        }
    }
}
