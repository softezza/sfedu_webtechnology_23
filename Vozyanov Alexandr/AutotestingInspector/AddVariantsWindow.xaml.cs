using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AutotestingInspectorSystem;

namespace AutotestingInspector
{
    /// <summary>
    /// Логика взаимодействия для AddLabWindow.xaml
    /// </summary>
    public partial class AddVariantsWindow : Window
    {
        private Editor _currentLabEditor;

        private AddVariantsWindow()
        {
            InitializeComponent();
        }

        public AddVariantsWindow(Editor lab)
        {
            InitializeComponent();

            _currentLabEditor = lab;

            LabName.Content = lab.LaboratoryWork.Name;

            if(lab.Options.Count > 18)
            {
                ScrollVariants.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            }
            else
            {
                ScrollVariants.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            }

            for (int i = 0; i < lab.Options.Count; i++)
            {
                VariantAdd(lab.LaboratoryWork.Options[i]);
            }
        }

        private void VariantDelete(int deletingVariantNum)
        {
            int row = Grid.GetRow(GridAddVar);
            int column = Grid.GetColumn(GridAddVar);
            int place = (row * 6) + column - 1;

            row = place / 6;
            column = place % 6;
            
            for (int i = deletingVariantNum + 2; i <= _currentLabEditor.Options.Count; i++)
            {
                int rowVariant = Grid.GetRow(MainVariantGrid.Children[i]);
                int columnVariant = Grid.GetColumn(MainVariantGrid.Children[i]);

                int varNumber = Convert.ToInt32(((MainVariantGrid.Children[i] as Grid).Children[2] as Button).Content);
                varNumber--;

                ((MainVariantGrid.Children[i] as Grid).Children[2] as Button).Content = varNumber.ToString(); 

                int varplace = (rowVariant * 6) + columnVariant - 1;

                rowVariant = varplace / 6;
                columnVariant = varplace % 6;                   

                Grid.SetColumn(MainVariantGrid.Children[i], columnVariant);
                Grid.SetRow(MainVariantGrid.Children[i], rowVariant);
            }

            MainVariantGrid.Children.Remove(MainVariantGrid.Children[deletingVariantNum+1]);    

            if(place >= 17 && column == 5)
            {
                MainVariantGrid.RowDefinitions.Remove(MainVariantGrid.RowDefinitions[MainVariantGrid.RowDefinitions.Count-1]);
                MainVariantGrid.Height -= 198;

                if (place < 18)
                {
                    ScrollVariants.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
            }

            Grid.SetColumn(GridAddVar, column);
            Grid.SetRow(GridAddVar, row);
        }       

        private void VariantAdd(LaboratoryWorkSystem.IViewOption tiedVariant)
        {
            int place;

            int row = Grid.GetRow(GridAddVar);
            int column = Grid.GetColumn(GridAddVar);
            place = (row * 6) + column + 1;

            Grid newVariant = VarCreate(column, row, tiedVariant);

            row = place / 6;
            column = place % 6;

            Grid.SetColumn(GridAddVar, column);
            Grid.SetRow(GridAddVar, row);
            MainVariantGrid.Children.Add(newVariant);

            if (place >= 18 && column == 0)
            {
                ScrollVariants.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                MainVariantGrid.RowDefinitions.Add(new RowDefinition());
                MainVariantGrid.Height += 198;
            }
        }

        private void ButtonHide(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private async void CloseCurrentEditor()
        {
            try
            {
                await CashData.Inspector.CloseEditor(_currentLabEditor);
            }
            catch(Exception ex)
            {
                CashData.Alert(ex.Message);
            }
        }

        private void ButtonBack(object sender, RoutedEventArgs e)
        {
            CloseCurrentEditor();

            var labCreationWindowBack = new LabCreationWindow();
            labCreationWindowBack.Top = this.Top;
            labCreationWindowBack.Left = this.Left;
            labCreationWindowBack.Show();
            Close();
        }

        private void ButtonDeleteVariant(object sender, RoutedEventArgs e)
        {
            var deleteWindow = new AlertWindow(WindowAlertType.Deleting);
            deleteWindow.ShowDialog();

            if(deleteWindow.DialogResult == true)
            {
                int number = Convert.ToInt32((sender as Button).Content);
                VariantDelete(number);

                LaboratoryWorkSystem.IViewOption tiedVariant = (sender as Button).Tag as LaboratoryWorkSystem.IViewOption;

                _currentLabEditor.RemoveOption(tiedVariant);
            }
        }

        private void ButtonOpenVariant(object sender, RoutedEventArgs e)
        {
            LaboratoryWorkSystem.IViewOption tiedVariant = (sender as Button).Tag as LaboratoryWorkSystem.IViewOption;

            OptionEditor optionEditor = _currentLabEditor.OpenOption(tiedVariant);

            var addVariantWindow = new AddTaskWindow(_currentLabEditor, optionEditor);
            addVariantWindow.Top = this.Top;
            addVariantWindow.Left = this.Left;
            addVariantWindow.Show();
            Close();
        }

        private void ButtonAddVariant(object sender, RoutedEventArgs e)
        {
            List<int> _numbersVariants = new List<int>();

            for (int i = 0; i < _currentLabEditor.Options.Count; i++)
            {
                _numbersVariants.Add(_currentLabEditor.LaboratoryWork.Options[i].Number);
            }

            var numberWindow = new NumberInputWindow(TypeWindowInput.Variant, _numbersVariants);
            numberWindow.ShowDialog();

            if (numberWindow.DialogResult == true)
            {
                _currentLabEditor.CreateOption(numberWindow.Number);
                VariantAdd(_currentLabEditor.LaboratoryWork.Options[_currentLabEditor.LaboratoryWork.Options.Count-1]);
            }            
        }

        private void ButtonChangeVariant(object sender, RoutedEventArgs e)
        {
            List<int> _numbersVariants = new List<int>();

            for (int i = 0; i < _currentLabEditor.Options.Count; i++)
            {
                _numbersVariants.Add(_currentLabEditor.LaboratoryWork.Options[i].Number);
            }

            LaboratoryWorkSystem.IViewOption tiedVariant = (sender as Button).Tag as LaboratoryWorkSystem.IViewOption;

            var numberWindow = new NumberInputWindow(TypeWindowInput.Variant, _numbersVariants);
            numberWindow.ShowDialog();

            if (numberWindow.DialogResult == true)
            {
                _currentLabEditor.UpdateNumberOption(tiedVariant, numberWindow.Number);

                AddVariantsWindow addLabWindow = new AddVariantsWindow(_currentLabEditor);
                addLabWindow.Top  = this.Top;
                addLabWindow.Left = this.Left;
                addLabWindow.Show();
                Close();
            }
        }

        private Grid VarCreate(int column, int row, LaboratoryWorkSystem.IViewOption tiedVariant)
        {
            Grid variant = new Grid();

            variant.HorizontalAlignment = HorizontalAlignment.Left;
            variant.Height = 177;
            variant.VerticalAlignment = VerticalAlignment.Top;
            variant.Width = 152;
            variant.Background = GridVarTemp.Background;

            Thickness margin = new Thickness();

            margin = variant.Margin;
            margin.Left = 10;
            margin.Top = 10;
            variant.Margin = margin;

            {
                TextBlock textBlockVar = new TextBlock();

                textBlockVar.Text = "Вариант " + tiedVariant.Number;
                textBlockVar.HorizontalAlignment = HorizontalAlignment.Left;

                margin = textBlockVar.Margin;
                margin.Left = 0;
                margin.Top = 136;
                margin.Bottom = 19;

                textBlockVar.Margin = margin;
                textBlockVar.Height = 22;
                textBlockVar.VerticalAlignment = VerticalAlignment.Top;
                textBlockVar.FontSize = 14;
                textBlockVar.Width = 152;
                textBlockVar.TextAlignment = TextAlignment.Center;
                textBlockVar.FontFamily = (GridVarTemp.Children[0] as TextBlock).FontFamily;

                variant.Children.Add(textBlockVar);
            }

            {
                Button button = new Button();

                button.Tag = tiedVariant;
                button.Content = _currentLabEditor.Options.Count.ToString();
                button.HorizontalAlignment = HorizontalAlignment.Left;

                button.Width  = 152;
                button.Height = 177;
                button.VerticalAlignment = VerticalAlignment.Top;

                button.Click += ButtonOpenVariant;

                button.Background = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                button.BorderBrush = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                button.Foreground = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));

                Grid.SetRow(button, row);
                Grid.SetColumn(button, column);

                variant.Children.Add(button);
            }

            // Delete button
            {
                Button buttonDelete = new Button();

                buttonDelete.Tag = tiedVariant;
                buttonDelete.Content = ((row * 6) + column ).ToString();
                buttonDelete.HorizontalAlignment = HorizontalAlignment.Left;

                margin = buttonDelete.Margin;
                margin.Left = 130;
                margin.Top = 155;
                margin.Right = 10;
                margin.Bottom = 7;

                buttonDelete.Margin = margin;
                buttonDelete.Width = 12;
                buttonDelete.Height = 15;
                buttonDelete.VerticalAlignment = VerticalAlignment.Top;

                buttonDelete.Click += ButtonDeleteVariant;

                buttonDelete.Background = (GridVarTemp.Children[2] as Button).Background;
                buttonDelete.BorderBrush = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                buttonDelete.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));

                Grid.SetRow(buttonDelete, row);
                Grid.SetColumn(buttonDelete, column);

                variant.Children.Add(buttonDelete);
            }

            // Change button
            {
                Button buttonChange = new Button();

                buttonChange.Tag = tiedVariant;
                buttonChange.Content = ((row * 6) + column + 1).ToString();
                buttonChange.HorizontalAlignment = HorizontalAlignment.Left;

                margin = buttonChange.Margin;
                margin.Left = 110;
                margin.Top = 155;
                margin.Right = 27;
                margin.Bottom = 7;

                buttonChange.Margin = margin;
                buttonChange.Width = 15;
                buttonChange.Height = 15;
                buttonChange.VerticalAlignment = VerticalAlignment.Top;

                buttonChange.Click += ButtonChangeVariant;

                buttonChange.Background = (GridVarTemp.Children[3] as Button).Background;
                buttonChange.BorderBrush = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));

                Grid.SetRow(buttonChange, row);
                Grid.SetColumn(buttonChange, column);

                variant.Children.Add(buttonChange);
            }

            Grid.SetRow(variant, row);
            Grid.SetColumn(variant, column);

            return variant;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void ButtonExit(object sender, RoutedEventArgs e)
        {
            CloseCurrentEditor();
            Close();
        }

        private async void ButtonSaveData(object sender, RoutedEventArgs e)
        {
           await _currentLabEditor.SaveLabWork();
        }
    }
}
