using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AutotestingInspectorSystem;

namespace AutotestingInspector
{
    public enum TypeDataWindow
    {
        None = 0,
        Kit,
        Input,
        Output
    }

    /// <summary>
    /// Логика взаимодействия для DataCreationWindow.xaml
    /// </summary>
    public partial class DataCreationWindow : Window
    {
        private const string dataTemp = "0";
        private int countKit = 0;
        private int currentKit = -1;              

        Editor _currentLabEditor;
        OptionEditor _currentVariantEditor;
        TaskEditor _currentTaskEditor;

        DataSetEditor _currentDataSetEditor;
        List<ProgramValidation.DataSet> _dataKits;

        private DataCreationWindow()
        {
            InitializeComponent();
        }

        public DataCreationWindow(Editor labEditor, OptionEditor variantEditor, TaskEditor taskEditor)
        {
            InitializeComponent();
            nameVar.Content = "Задание №" + taskEditor.Task.Number;

            _currentLabEditor = labEditor;
            _currentVariantEditor = variantEditor;
            _currentTaskEditor = taskEditor;

            _dataKits = taskEditor.Task.GetDataSets();
            DescriptionTextBox.Text = taskEditor.Task.Description;

            for (int i = 0; i < _dataKits.Count; i++)
            {
                AddKitFront();
            }
        }

        private Grid KitCreate(int row, int kitNum)
        {
            Grid data = new Grid();

            data.HorizontalAlignment = HorizontalAlignment.Left;
            data.Height = 37;
            data.VerticalAlignment = VerticalAlignment.Top;
            data.Width = 311;
            data.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xE4, 0xE8, 0xF7));

            Thickness margin = new Thickness();

            margin = data.Margin;
            margin.Left = 0;
            margin.Top = 7;
            data.Margin = margin;

            {
                TextBlock textBoxDataKit = new TextBlock();

                textBoxDataKit.Text = "Набор " + kitNum;
                textBoxDataKit.HorizontalAlignment = HorizontalAlignment.Left;
                textBoxDataKit.TextWrapping = TextWrapping.Wrap;

                margin = textBoxDataKit.Margin;
                margin.Left = 10;
                margin.Top = 5;

                textBoxDataKit.Margin = margin;
                textBoxDataKit.Height = 32;
                textBoxDataKit.VerticalAlignment = VerticalAlignment.Top;
                textBoxDataKit.FontSize = 17;
                textBoxDataKit.Width = 174;
                textBoxDataKit.FontFamily = (GridKitDataTemp.Children[0] as TextBlock).FontFamily;
                textBoxDataKit.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x52, 0x4F, 0x4F));

                data.Children.Add(textBoxDataKit);
            }

            {
                Button button = new Button();

                button.Content = kitNum.ToString();
                button.HorizontalAlignment = HorizontalAlignment.Left;

                button.Width = 311;
                button.Height = 37;
                button.VerticalAlignment = VerticalAlignment.Top;

               
                button.Click += KitButton;

                button.Background = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                button.BorderBrush = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                button.Foreground = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));

                Grid.SetRow(button, row);

                data.Children.Add(button);
            }

            // Delete button
            {
                Button buttonDelete = new Button();

                buttonDelete.Content = kitNum.ToString();
                buttonDelete.HorizontalAlignment = HorizontalAlignment.Left;

                margin = buttonDelete.Margin;
                margin.Left = 275;
                margin.Top = 5;

                buttonDelete.Margin = margin;
                buttonDelete.Width = 20;
                buttonDelete.Height = 26;
                buttonDelete.VerticalAlignment = VerticalAlignment.Top;

                buttonDelete.Click += (sender, e) => { ButtonDelete(sender as Button, TypeDataWindow.Kit); };

                buttonDelete.Unloaded += (sender, e) => { };

                buttonDelete.Background = (GridKitDataTemp.Children[2] as Button).Background;
                buttonDelete.BorderBrush = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));

                Grid.SetRow(buttonDelete, row);

                data.Children.Add(buttonDelete);
            }

            Grid.SetRow(data, row);

            return data;
        }

        private Grid InputCreate(int row, string text = dataTemp)
        {
            Grid data = new Grid();

            data.HorizontalAlignment = HorizontalAlignment.Left;
            data.Height = 37;
            data.VerticalAlignment = VerticalAlignment.Top;
            data.Width = 369;
            data.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xE4, 0xE8, 0xF7));

            Thickness margin = new Thickness();

            margin = data.Margin;
            margin.Left = 0;
            margin.Top = 7;
            data.Margin = margin;

            {
                TextBox textBoxDataInput = new TextBox();

                textBoxDataInput.HorizontalAlignment = HorizontalAlignment.Left;
                textBoxDataInput.TextWrapping = TextWrapping.Wrap;
                textBoxDataInput.Text = text;
                textBoxDataInput.Tag = row;

                margin = textBoxDataInput.Margin;
                margin.Left = 9;

                textBoxDataInput.TextChanged += (sender, e) =>
                {
                    DataUpdate(sender, (sender as TextBox).Text, _currentDataSetEditor.InputEditor);
                };

                textBoxDataInput.Margin = margin;
                textBoxDataInput.Height = 37;
                textBoxDataInput.VerticalAlignment = VerticalAlignment.Top;
                textBoxDataInput.FontSize = 17;
                textBoxDataInput.Width = 325;
                textBoxDataInput.FontFamily = (GridInputDataTemp.Children[0] as TextBox).FontFamily;
                textBoxDataInput.CaretBrush = new SolidColorBrush(Colors.Black);
                textBoxDataInput.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x52, 0x4F, 0x4F));
                textBoxDataInput.BorderBrush = new SolidColorBrush(Colors.Transparent);

                data.Children.Add(textBoxDataInput);
            }

            {
                Button buttonDelete = new Button();

                buttonDelete.Content = row.ToString();
                buttonDelete.HorizontalAlignment = HorizontalAlignment.Left;

                margin = buttonDelete.Margin;
                margin.Left = 334;
                margin.Top = 5;

                buttonDelete.Margin = margin;
                buttonDelete.Width = 20;
                buttonDelete.Height = 26;

                buttonDelete.VerticalAlignment = VerticalAlignment.Top;

                buttonDelete.Click += (sender, e) => { ButtonDelete(sender as Button, TypeDataWindow.Input); };

                buttonDelete.Background = (GridInputDataTemp.Children[1] as Button).Background;
                buttonDelete.BorderBrush = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));

                Grid.SetRow(buttonDelete, row);

                data.Children.Add(buttonDelete);
            }

            Grid.SetRow(data, row);

            return data;
        }

        private Grid OutputCreate(int row, string text = dataTemp)
        {
            Grid data = new Grid();

            data.HorizontalAlignment = HorizontalAlignment.Left;
            data.Height = 37;
            data.VerticalAlignment = VerticalAlignment.Top;
            data.Width = 311;
            data.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xE4, 0xE8, 0xF7));

            Thickness margin = new Thickness();

            margin = data.Margin;
            margin.Left = 0;
            margin.Top = 7;
            data.Margin = margin;

            {
                TextBox textBoxDataOutput = new TextBox();
                                
                textBoxDataOutput.HorizontalAlignment = HorizontalAlignment.Left;
                textBoxDataOutput.TextWrapping = TextWrapping.Wrap;
                textBoxDataOutput.Text = text;
                textBoxDataOutput.Tag = row;

                margin = textBoxDataOutput.Margin;
                margin.Left = 9;

                textBoxDataOutput.TextChanged += (sender, e) =>
                {
                    DataUpdate(sender, (sender as TextBox).Text, _currentDataSetEditor.OutputEditor);
                };

                textBoxDataOutput.Margin = margin;
                textBoxDataOutput.Height = 37;
                textBoxDataOutput.VerticalAlignment = VerticalAlignment.Top;
                textBoxDataOutput.FontSize = 17;
                textBoxDataOutput.Width = 265;
                textBoxDataOutput.FontFamily = (GridInputDataTemp.Children[0] as TextBox).FontFamily;
                textBoxDataOutput.CaretBrush = new SolidColorBrush(Colors.Black);
                textBoxDataOutput.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x52, 0x4F, 0x4F));
                textBoxDataOutput.BorderBrush = new SolidColorBrush(Colors.Transparent);

                data.Children.Add(textBoxDataOutput);
            }

            {
                Button buttonDelete = new Button();

                buttonDelete.Content = row.ToString();
                buttonDelete.HorizontalAlignment = HorizontalAlignment.Left;

                margin = buttonDelete.Margin;
                margin.Left = 276;
                margin.Top = 5;

                buttonDelete.Margin = margin;
                buttonDelete.Width = 20;
                buttonDelete.Height = 26;
                buttonDelete.VerticalAlignment = VerticalAlignment.Top;

                buttonDelete.Click += (sender, e) => { ButtonDelete(sender as Button, TypeDataWindow.Output); };

                buttonDelete.Background = (GridInputDataTemp.Children[1] as Button).Background;
                buttonDelete.BorderBrush = new SolidColorBrush(Colors.Transparent);

                Grid.SetRow(buttonDelete, row);

                data.Children.Add(buttonDelete);
            }

            Grid.SetRow(data, row);

            return data;
        }

        private void ButtonDelete(Button button, TypeDataWindow type)
        {
            var deleteWindow = new AlertWindow(WindowAlertType.Deleting, "Вы уверены, удалённые данные не вернуть?");
            deleteWindow.ShowDialog();
            
            if(deleteWindow.DialogResult == true)
            {
                int num = Convert.ToInt32(button.Content);                

                switch (type)
                {
                    case TypeDataWindow.Kit:
                        KitDelete(num);
                        break;
                    case TypeDataWindow.Input:
                        InputDelete(num);
                        break;
                    case TypeDataWindow.Output:
                        OutputDelete(num);
                        break;
                }
            }
        }

        private void KitDelete(int numKit)
        {
            if (countKit >= 7)
            {
                MainKitGrid.RowDefinitions.Remove(MainKitGrid.RowDefinitions[MainKitGrid.RowDefinitions.Count-1]);
                MainKitGrid.Height -= 51;
            }

            bool isCurrentKitReplaced = false;

            for (int i = numKit; i < countKit; i++)
            {
                int rowKit = Grid.GetRow(MainKitGrid.Children[i]);

                int kitNumber = Convert.ToInt32(((MainKitGrid.Children[i] as Grid).Children[2] as Button).Content);

                if (kitNumber == currentKit)
                {
                    currentKit--;
                    isCurrentKitReplaced = true; 
                }

                kitNumber--;

                ((MainKitGrid.Children[i] as Grid).Children[2] as Button).Content = kitNumber.ToString();
                ((MainKitGrid.Children[i] as Grid).Children[1] as Button).Content = kitNumber.ToString();
                ((MainKitGrid.Children[i] as Grid).Children[0] as TextBlock).Text = "Набор " + kitNumber;               

                 rowKit--;

                Grid.SetRow(MainKitGrid.Children[i], rowKit);
            }

            MainKitGrid.Children.Remove(MainKitGrid.Children[numKit - 1]);

            countKit--;
            _currentTaskEditor.RemoveDataSet(numKit - 1);

            if (countKit == 0)
            {
                AddInput.IsEnabled = false;
                AddOutput.IsEnabled = false;
            }

            if(!isCurrentKitReplaced && numKit == currentKit || isCurrentKitReplaced && numKit == currentKit+1)
            {
                currentKit = -1;
                AddInput.IsEnabled = false;
                AddOutput.IsEnabled = false;
                _currentDataSetEditor = null;
                ClearInOut();
            }
        }

        private void InputDelete(int numInput)
        {
            if ((_currentDataSetEditor.InputEditor as DataSetEditor.InputDataEditor).InputData.Count >= 7)
            {
                MainInputGrid.RowDefinitions.Remove(MainInputGrid.RowDefinitions[MainInputGrid.RowDefinitions.Count - 1]);
                MainInputGrid.Height -= 51;
            }

            for (int i = numInput + 1; i < (_currentDataSetEditor.InputEditor as DataSetEditor.InputDataEditor).InputData.Count; i++)
            {
                int inputNumber = Convert.ToInt32(((MainInputGrid.Children[i] as Grid).Children[1] as Button).Content) - 1;

                ((MainInputGrid.Children[i] as Grid).Children[0] as TextBox).Tag = inputNumber.ToString();
                ((MainInputGrid.Children[i] as Grid).Children[1] as Button).Content = inputNumber.ToString();

                Grid.SetRow(MainInputGrid.Children[i], inputNumber);
            }

            MainInputGrid.Children.Remove(MainInputGrid.Children[numInput]);

            if (_currentDataSetEditor != null)
            {
                _currentDataSetEditor.InputEditor.RemoveUnit(numInput);
            }
        }

        private void OutputDelete(int numOutput)
        {
            if ((_currentDataSetEditor.OutputEditor as DataSetEditor.ExpectedOutputDataEditor).ExpectedOutputData.Count >= 7)
            {
                MainOutputGrid.RowDefinitions.Remove(MainOutputGrid.RowDefinitions[MainOutputGrid.RowDefinitions.Count - 1]);
                MainOutputGrid.Height -= 51;
            }

            for (int i = numOutput + 1; i < (_currentDataSetEditor.OutputEditor as DataSetEditor.ExpectedOutputDataEditor).ExpectedOutputData.Count; i++)
            {
                int outputNumber = Convert.ToInt32(((MainOutputGrid.Children[i] as Grid).Children[1] as Button).Content) - 1;

                ((MainOutputGrid.Children[i] as Grid).Children[0] as TextBox).Tag = outputNumber.ToString();
                ((MainOutputGrid.Children[i] as Grid).Children[1] as Button).Content = outputNumber.ToString();

                Grid.SetRow(MainOutputGrid.Children[i], outputNumber);
            }

            MainOutputGrid.Children.Remove(MainOutputGrid.Children[numOutput]);

            if (_currentDataSetEditor != null)
            {
                _currentDataSetEditor.OutputEditor.RemoveUnit(numOutput);
            }
        }

        private void ButtonHide(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonAddKit(object sender, RoutedEventArgs e)
        {
            AddKitFront();
            _currentTaskEditor.CreateDataSet();
        }

        private void AddKitFront()
        {
            if (countKit >= 6)
            {
                MainKitGrid.RowDefinitions.Add(new RowDefinition());
                MainKitGrid.Height += 51;
            }

            MainKitGrid.Children.Add(KitCreate(countKit, countKit + 1));

            countKit++;
        }

        private void KitButton(object sender, RoutedEventArgs e)
        {
            int numKit = Convert.ToInt32((sender as Button).Content);
            ChoiceKit(numKit);
        }

        private void ChoiceKit(int kitNumber)
        {
            if(currentKit > 0)
            {
                (MainKitGrid.Children[currentKit-1] as Grid).Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xE4, 0xE8, 0xF7));
            }

            (MainKitGrid.Children[kitNumber-1] as Grid).Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xCF, 0xC7, 0xCE));

            if (AddInput.IsEnabled == false || AddOutput.IsEnabled == false)
            {
                AddInput.IsEnabled = true;
                AddOutput.IsEnabled = true;
            }

            currentKit = kitNumber;
            _currentDataSetEditor = _currentTaskEditor.OpenDataSet(kitNumber - 1);

            ClearInOut();

            for (int i = 0; i < (_currentDataSetEditor.InputEditor as DataSetEditor.InputDataEditor).InputData.Count; i++)
            {
                if ((_currentDataSetEditor.InputEditor as DataSetEditor.InputDataEditor).InputData.Count >= 6)
                {
                    MainInputGrid.RowDefinitions.Add(new RowDefinition());
                    MainInputGrid.Height += 51;
                }

                MainInputGrid.Children.Add(InputCreate(i, (_currentDataSetEditor.InputEditor as DataSetEditor.InputDataEditor).InputData[i].Value.ToString()));
            }

            for (int i = 0; i < (_currentDataSetEditor.OutputEditor as DataSetEditor.ExpectedOutputDataEditor).ExpectedOutputData.Count; i++)
            {
                if ((_currentDataSetEditor.OutputEditor as DataSetEditor.ExpectedOutputDataEditor).ExpectedOutputData.Count >= 6)
                {
                    MainOutputGrid.RowDefinitions.Add(new RowDefinition());
                    MainOutputGrid.Height += 51;
                }

                MainOutputGrid.Children.Add(OutputCreate(i, (_currentDataSetEditor.OutputEditor as DataSetEditor.ExpectedOutputDataEditor).ExpectedOutputData[i].Value.ToString()));
            }
        }

        private async void ButtonSaveData(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DescriptionTextBox.Text != "")
                {
                    _currentTaskEditor.UpdateDescription(DescriptionTextBox.Text);
                }

                _currentTaskEditor.SaveStatus();
                await _currentLabEditor.SaveLabWork();
            }
            catch (Exception ex)
            {
                CashData.Alert(ex.Message);
            }
        }

        private void ButtonAddInputData(object sender, RoutedEventArgs e)
        {
            if ((_currentDataSetEditor.InputEditor as DataSetEditor.InputDataEditor).InputData.Count >= 6)
            {
                MainInputGrid.RowDefinitions.Add(new RowDefinition());
                MainInputGrid.Height += 51;
            }

            if (_currentDataSetEditor != null)
            {
                _currentDataSetEditor.InputEditor.CreateUnit();
            }

            MainInputGrid.Children.Add(InputCreate((_currentDataSetEditor.InputEditor as DataSetEditor.InputDataEditor).InputData.Count-1));         
        }

        private void ButtonAddOutputData(object sender, RoutedEventArgs e)
        {
            if ((_currentDataSetEditor.OutputEditor as DataSetEditor.ExpectedOutputDataEditor).ExpectedOutputData.Count >= 6)
            {
                MainOutputGrid.RowDefinitions.Add(new RowDefinition());
                MainOutputGrid.Height += 51;
            }

            if (_currentDataSetEditor != null)
            {
                _currentDataSetEditor.OutputEditor.CreateUnit();
            }

            MainOutputGrid.Children.Add(OutputCreate((_currentDataSetEditor.OutputEditor as DataSetEditor.ExpectedOutputDataEditor).ExpectedOutputData.Count-1));          
        }

        private void DataUpdate(object sender, string value, DataSetEditor.IDataEditor editor)
        {
            int numData = Convert.ToInt32((sender as TextBox).Tag);

            ProgramValidation.IUnit unit;

            if (double.TryParse(value, out double resultDouble))
            {
                unit = new ProgramValidation.UnitDouble(resultDouble);
            }
            else if (int.TryParse(value, out int resultInt))
            {
                unit = new ProgramValidation.UnitInt(resultInt);
            }
            else if (char.TryParse(value, out char resultChar))
            {
                unit = new ProgramValidation.UnitChar(resultChar);
            }
            else
            {
                unit = new ProgramValidation.UnitString(value);
            }

            editor.UpdateUnit(numData, unit);
        }

        private void ButtonBack(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = new AddTaskWindow(_currentLabEditor, _currentVariantEditor);
            addTaskWindow.Top = this.Top;
            addTaskWindow.Left = this.Left;
            addTaskWindow.Show();
            Close();
        }       

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
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

        private void ButtonExit(object sender, RoutedEventArgs e)
        {
            CloseCurrentEditor();
            Close();
        }

        private void ClearInOut()
        {
            MainInputGrid.Children.Clear();
            MainOutputGrid.Children.Clear();

            MainOutputGrid.RowDefinitions.Clear();
            MainInputGrid.RowDefinitions.Clear();

            for (int i = 0; i < 6; i++)
            {
                MainOutputGrid.RowDefinitions.Add(new RowDefinition());
                MainInputGrid.RowDefinitions.Add(new RowDefinition());
            }

            MainOutputGrid.Height = 306;
            MainInputGrid.Height = 306;
        }
    }
}