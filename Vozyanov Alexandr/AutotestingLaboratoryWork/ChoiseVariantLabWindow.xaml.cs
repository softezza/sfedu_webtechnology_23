using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Autotesting
{
    /// <summary>
    /// Логика взаимодействия для ChoiseVariantLabWindow.xaml
    /// </summary>
    public partial class ChoiseVariantLabWindow : Window
    {
        private List<LaboratoryWorkSystem.IViewOption> variants;
        private List<Grid> variantsButton;
        private int currentLab;

        private ChoiseVariantLabWindow() { }

        public ChoiseVariantLabWindow(LaboratoryWorkSystem.IViewLaboratoryWork lab, int currentNumLab)
        {
            InitializeComponent();            

            variantsButton = new List<Grid>();

            labelVersion.Content = "ver " + CashData.Version;
            currentLab = currentNumLab;
            labelFIO.Text = CashData.FIO;
            
            this.variants = lab.Options.ToList(); 
            labelGroup.Content = CashData.group;

            ScrollTasks.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;

            for (int i = 0; i < lab.Options.Count; i++)
            {
                int row = 0;
                int colunm = i;

                if (colunm / 7 >= 1)
                {
                    row = colunm / 7;
                    colunm = colunm % 7;
                }

                if (i >= 28 && colunm == 0)
                {
                    VarGrid.RowDefinitions.Add(new RowDefinition());
                    VarGrid.Height += 144;
                }

                variantsButton.Add(VarButton("Вариант №" + lab.Options[i].Number, row, colunm));
                VarGrid.Children.Add(variantsButton[i]);
            }
        }

        private Grid VarButton(string labelContent, int row, int colunum)
        {
            Thickness margin;
            Grid varButton = new Grid();

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

                varButton.Children.Add(image);
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
                label.FontFamily = (VarOne.Children[1] as Label).FontFamily;

                varButton.Children.Add(label);
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

                button.Click += ButtonVariant_Click;

                button.Background = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                button.BorderBrush = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));

                Grid.SetRow(button, row);
                Grid.SetColumn(button, colunum);

                varButton.Children.Add(button);
            }

            Grid.SetRow(varButton, row);
            Grid.SetColumn(varButton, colunum);

            return varButton;
        }

        private void ButtonVariant_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int currentVarnum = Grid.GetColumn(button) + (Grid.GetRow(button) * 7);
            Labs_Testing labsCheckWindow = new Labs_Testing(variants[currentVarnum], currentLab);
            labsCheckWindow.Top = Top;
            labsCheckWindow.Left = Left;
            labsCheckWindow.Show();
            Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void ButtonExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            LabsBase labsListWindow = new LabsBase();
            labsListWindow.Top = Top;
            labsListWindow.Left = Left;
            labsListWindow.Show();

            Close();
        }
    }
}