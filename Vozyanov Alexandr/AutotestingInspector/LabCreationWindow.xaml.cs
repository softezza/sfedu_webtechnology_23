
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AutotestingInspectorSystem;


namespace AutotestingInspector
{
    public static class CashData
    {
        public static Inspector Inspector;

        static string _nameFolderAppData = @"Autotesting Inspector";
        static string _nameFileLogData = @"Log_Data_Autotesting_Inspector.txt";

        static DirectoryInfo _directoryAppData;
        public static FileInfo _logAutotestingInspectorFile;

        private static void Initialize()
        {
            _directoryAppData = new DirectoryInfo(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), _nameFolderAppData));
            _logAutotestingInspectorFile = new FileInfo(System.IO.Path.Combine(_directoryAppData.FullName, _nameFileLogData));

            if (!_directoryAppData.Exists) Directory.CreateDirectory(_directoryAppData.FullName);
        }

        public static void WriteLog(string errorMessage)
        {
            Initialize();

            using (StreamWriter fstream = new StreamWriter(_logAutotestingInspectorFile.FullName, true))
            {
                string text = DateTime.Now.ToString("g") + errorMessage + '\n';
                fstream.Write(text);
            }
        }

        public static void Alert(string message)
        {
            AlertWindow alert = new AlertWindow(WindowAlertType.Warning, message);
            alert.ShowDialog();
        }
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class LabCreationWindow : Window
    {
        private Inspector _inspector;

        public LabCreationWindow()
        {
            InitializeComponent();
            LoadInspector();
        }

        private void ButtonExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonOpenLab(object sender, RoutedEventArgs e)
        {
            OpenLabFile();
        }

        private void OpenLab(object sender, RoutedEventArgs e)
        {
            string path = (sender as Button).Content.ToString();
            OpenLab(path);
        }

        private void LabNew(object sender, RoutedEventArgs e)
        {
            CreateNewLab();
        }

        private void Menu(object sender, RoutedEventArgs e)
        {
            if (MenuGrid.Visibility == Visibility.Hidden)
            {
                MenuGrid.Visibility = Visibility.Visible;
                MenuButton.Background = GetBackground("Resources/CancelButtonW.png");
            }
            else
            {
                MenuGrid.Visibility = Visibility.Hidden;
                MenuButton.Background = GetBackground("Resources/Add buttonW.png");
            }
        }

        private ImageBrush GetBackground(string pathToImage)
        {
            return new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), pathToImage)));
        }

        private void ButtonHide(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private Grid LabCreate(int row, DataLabWorkFile labCont)
        {
            Grid lab = new Grid();

            lab.HorizontalAlignment = HorizontalAlignment.Left;
            lab.Height = 74;
            lab.VerticalAlignment = VerticalAlignment.Top;
            lab.Width = 495;
            lab.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xF7, 0xF8, 0xFC));

            Thickness margin = new Thickness();

            {
                Image image = new Image();

                image.HorizontalAlignment = HorizontalAlignment.Left;
                image.Height = 44;
                margin = image.Margin;
                margin.Left = 10;
                margin.Top = 16;
                image.Margin = margin;
                image.VerticalAlignment = VerticalAlignment.Top;
                image.Width = 50;
                image.Source = new BitmapImage(new Uri("Resources/LabIcon.png", UriKind.Relative));
                
                image.Cursor = Cursors.Hand;

                lab.Children.Add(image);
            }

            {
                Label labelName = CreateLabel();

                labelName.Content = labCont.Name;

                margin = labelName.Margin;
                margin.Left = 86;
                margin.Top = 10;

                labelName.Margin = margin;
                labelName.Height = 36;
                labelName.FontSize = 23;
                labelName.Width = 300;
                labelName.FontFamily = (LabTempGrid.Children[1] as Label).FontFamily;

                lab.Children.Add(labelName);
            }

            {
                Label labelPath = CreateLabel();

                labelPath.Content = labCont.Path;

                margin = labelPath.Margin;
                margin.Left = 91;
                margin.Top = 36;

                labelPath.Margin = margin;
                labelPath.Width = 300;
                labelPath.Height = 22;
                labelPath.FontSize = 11;
                labelPath.FontFamily = (LabTempGrid.Children[2] as Label).FontFamily;

                lab.Children.Add(labelPath);
            }

            {
                Label labelData = CreateLabel();

                labelData.Content = labCont.DateTime.ToString("dd/MM/yyyy");
                
                margin = labelData.Margin;
                margin.Left = 403;
                margin.Top = 34;
                margin.Right = 16;

                labelData.Margin = margin;
                labelData.Height = 24;
                labelData.FontSize = 11;
                labelData.FontFamily = (LabTempGrid.Children[3] as Label).FontFamily;
                labelData.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x7D, 0x7D, 0x7D));
                labelData.Width = 60;

                lab.Children.Add(labelData);
            }

            {
                Button button = new Button();

                button.Content = labCont.Path.ToString();
                button.HorizontalAlignment = HorizontalAlignment.Left;

                button.Width = 495;
                button.Height = 74;
                button.VerticalAlignment = VerticalAlignment.Top;

                button.Click += OpenLab;

                button.Background  = new SolidColorBrush(Colors.Transparent);
                button.BorderBrush = new SolidColorBrush(Colors.Transparent);
                button.Foreground  = new SolidColorBrush(Colors.Transparent);

                Grid.SetRow(button, row);

                lab.Children.Add(button);
            }

            Grid.SetRow(lab, row);

            return lab;
        }

        private Label CreateLabel(/*string content, Color foregroundColor,FontFamily font, int fontSize, int width = 0, int heigh = 0, int leftM = 0, int rightM = 0, int topM = 0*/)
        {
            Label label = new Label();

            //label.Content = content;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            //Thickness margin;
            //margin = label.Margin;
            //margin.Left = leftM;
            //margin.Top = topM;
            //margin.Right = rightM;

            //label.Margin = margin;
            //label.Height = heigh;
            label.VerticalAlignment = VerticalAlignment.Top;
            //label.FontSize = fontSize;
            //label.FontFamily = font;
            //label.Foreground = new SolidColorBrush(foregroundColor);
            //label.Width = width;

            return label;
        }

        private void LoadHistory()
        {
            var history = _inspector.HistoryLabWorks;

            (LabTempGrid.Children[1] as Label).Content = history.Count;

            if (history.Count >= 7)
            {
                ScrollLab.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            }
            else
            {
                ScrollLab.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            }

            for (int i = 0; i < history.Count; i++)
            {
                if (i >= 7)
                {
                    MainLabGrid.RowDefinitions.Add(new RowDefinition());
                    MainLabGrid.Height += 84;
                }

                Grid newlab = LabCreate(i, history[i]);
                MainLabGrid.Children.Add(newlab);
            }            
        }

        private async void LoadInspector()
        {
            try
            { 
                _inspector = await Inspector.CreateInspector();
                CashData.Inspector = _inspector;
                LoadHistory();
            }
            catch(Exception e)
            {
                CashData.Alert(e.Message);
            }    
        }

        private async void OpenLab(string path)
        {
            try
            {
                var editor = await _inspector.OpenLabWork(@path);
                AddVariantsWindow addLabWindow = new AddVariantsWindow(editor);
                addLabWindow.Top = this.Top;
                addLabWindow.Left = this.Left;
                addLabWindow.Show();                
            }
            catch(Exception e)
            {
                CashData.Alert(e.Message);
            }

            Close();
        }

        private void OpenLabFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = $"Lab files (*{LaboratoryWorkSystem.LaboratoryWork.FileHelper.Extension})|*{LaboratoryWorkSystem.LaboratoryWork.FileHelper.Extension}",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                OpenLab(openFileDialog.FileName);
            }       
        }

        private void CreateNewLab()
        {
            var addLabWindow = new LabInitWindow(Top, Left);

            if (addLabWindow.ShowDialog() == true)
            {
                Close();
            }
        }      
    }
}
