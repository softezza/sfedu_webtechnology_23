using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Autotesting
{
    /// <summary>
    /// Логика взаимодействия для LabsBase.xaml
    /// </summary>
    public partial class LabsBase : Window
    {
        List<Grid> labsButton;
 
        string path = "";

        public LabsBase()
        {
            InitializeComponent();
            labsButton = new List<Grid>();

            labelFIO.Text   = CashData.FIO;
            labelGroup.Content = CashData.group;
            labelVersion.Content = "ver " + CashData.Version;

            LabsButtonController();
        }

        private Grid LabButton(string labelContent, int row, int colunum)
        {
            Thickness margin;

            Grid labButton = new Grid();

            {
                Image image = new Image();

                image.HorizontalAlignment = HorizontalAlignment.Left;
                image.Height = 130;

                margin = image.Margin;
                margin.Left = 15;
                margin.Top = 5;
                image.Margin = margin;
                image.VerticalAlignment = VerticalAlignment.Top;
                image.Width = 112;
                image.Source = new BitmapImage(new Uri("Resources/Lab.png", UriKind.Relative));
                image.Cursor = Cursors.Hand;

                labButton.Children.Add(image);
            }

            {
                Label label = new Label();

                label.Content = labelContent;
                label.HorizontalAlignment = HorizontalAlignment.Left;

                margin = label.Margin;
                margin.Left = 24;
                margin.Top = 92;
                margin.Right = 16;

                label.Margin = margin;
                label.Height = 43;
                label.VerticalAlignment = VerticalAlignment.Top;
                label.FontSize = 14;
                label.FontFamily = (labTemp.Children[1] as Label).FontFamily;

                labButton.Children.Add(label);
            }

            {
                Button button = new Button();

                button.Content = "";
                button.HorizontalAlignment = HorizontalAlignment.Left;

                margin = button.Margin;
                margin.Left = 15;
                margin.Top = 5;

                button.Margin = margin;
                button.Width = 112;
                button.Height = 129;
                button.VerticalAlignment = VerticalAlignment.Top;

                button.Click += ButtonLab_Click;

                button.Background = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                button.BorderBrush = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));

                Grid.SetRow(button, row);
                Grid.SetColumn(button, colunum);

                labButton.Children.Add(button);
            }

            Grid.SetRow(labButton, row);
            Grid.SetColumn(labButton, colunum);

            //labButton.Background = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));

            return labButton;
        }

        private void LabsButtonController()
        {
            if(CashData.labsLW.Count >= 28)
            {
                ScrollLab.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            }
            else
            {
                ScrollLab.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            }

            for (int i = 0; i < CashData.labsLW.Count; i++)
            {
                AddLab(i);
            }
        }
        
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        public void DragDropFile(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];

                if (files != null && files.Any())
                {
                    FileHandler(files.First());                    
                }
            }
        }

        private void AddLab(int place)
        {
            int  row = place / 7;
            int  colunm = place % 7;

            if (place >= 28 && colunm == 0)
            {
                LabGrid.RowDefinitions.Add(new RowDefinition());
                LabGrid.Height += 144;
            }

            labsButton.Add(LabButton(CashData.labsLW[place].Name, row, colunm));
            LabGrid.Children.Add(labsButton[place]);
        }

        private void FileHandler(string fileName)
        {
            path = fileName;
            fileName = Path.GetFileName(fileName);

            if (Path.GetExtension(fileName) != LaboratoryWorkSystem.LaboratoryWork.FileHelper.Extension)
            {
                labelDrag.Content = "Перетащите сюда файл лабораторной работы... Неверное разрешение файла: " + fileName;
                return; 
            }

            DeserializedFile(path);            

            labelDrag.Content = "Перетащите сюда файл лабораторной работы... Файл загружен: " + fileName;
        }

        private async void DeserializedFile(string path)
        {
            LaboratoryWorkSystem.IViewLaboratoryWork labWork = await LaboratoryWorkSystem.LaboratoryWork.FileHelper.LoadLaboratoryWork(path);
            CashData.labsLW.Add(labWork);
            CashData.IsCompleteTasks.Add(labWork, new Dictionary<LaboratoryWorkSystem.IViewOption, Dictionary<LaboratoryWorkSystem.IViewTask, bool>>());

            for (int i = 0; i < labWork.Options.Count; i++)
            {
                CashData.IsCompleteTasks[labWork].Add(labWork.Options[i], new Dictionary<LaboratoryWorkSystem.IViewTask, bool>());

                for (int j = 0; j < labWork.Options[i].Tasks.Count; j++)
                {
                    CashData.IsCompleteTasks[labWork][labWork.Options[i]].Add(labWork.Options[i].Tasks[j], false);
                }
            }

            if (CashData.labsLW.Count >= 28)
            {
                ScrollLab.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            }
            else
            {
                ScrollLab.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            }

            AddLab(CashData.labsLW.Count - 1);
        }

        private void ButtonExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonLab_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int currentLabnum = Grid.GetColumn(button) + (Grid.GetRow(button) * 7);
            
            ChoiseVariantLabWindow labsTestWindow = new ChoiseVariantLabWindow(CashData.labsLW[currentLabnum], currentLabnum);
            labsTestWindow.Top  = Top;
            labsTestWindow.Left = Left;
            labsTestWindow.Show();
            Close();
        }
    }
}