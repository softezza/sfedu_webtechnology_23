using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AutotestingInspectorSystem;

namespace AutotestingInspector
{
    /// <summary>
    /// Логика взаимодействия для AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        private Editor _currentLabEditor;
        private OptionEditor _currentOptionEditor;

        private AddTaskWindow()
        {
            InitializeComponent();
        }

        public AddTaskWindow( Editor editorlab, OptionEditor optionEditor)
        {
            InitializeComponent();

            _currentLabEditor = editorlab;
            _currentOptionEditor = optionEditor;
            VarName.Content = "Вариант №" + optionEditor.Option.Number;

            if(optionEditor.Option.Tasks.Count > 18)
            {
                ScrollTasks.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            }

            for (int i = 0; i < optionEditor.Option.Tasks.Count; i++)
            {
                CreateTask(optionEditor.Option.Tasks[i]);
            }
        }

        private void ButtonHide(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonBack(object sender, RoutedEventArgs e)
        {
            var varCreationWindowBack = new AddVariantsWindow(_currentLabEditor);
            varCreationWindowBack.Top = this.Top;
            varCreationWindowBack.Left = this.Left;
            varCreationWindowBack.Show();
            Close();
        }

        private void ButtonDelete(object sender, RoutedEventArgs e)
        {
            var deletingWindow = new AlertWindow(WindowAlertType.Deleting, "Вы уверены что хотите удалить это задание?");
            deletingWindow.ShowDialog();

            if (deletingWindow.DialogResult == true)
            { 
                LaboratoryWorkSystem.IViewTask tiedTask = (sender as Button).Tag as LaboratoryWorkSystem.IViewTask;
                int number = Convert.ToInt32((sender as Button).Content);
                TaskDelete(number);
                _currentOptionEditor.RemoveTask(tiedTask);
            }
        }

        private void ButtonTask(object sender, RoutedEventArgs e)
        {
            LaboratoryWorkSystem.IViewTask tiedtask = (sender as Button).Tag as LaboratoryWorkSystem.IViewTask;

            TaskEditor taskEditor = _currentOptionEditor.OpenTask(tiedtask);

            var addTaskWindow = new DataCreationWindow(_currentLabEditor, _currentOptionEditor, taskEditor);
            addTaskWindow.Top = this.Top;
            addTaskWindow.Left = this.Left;
            addTaskWindow.Show();
            Close();
        }

        private void ButtonAddTask(object sender, RoutedEventArgs e)
        {
            List<int> _numbersTasks = new List<int>();

            for (int i = 0; i < _currentOptionEditor.Option.Tasks.Count; i++)
            {
                _numbersTasks.Add(_currentOptionEditor.Option.Tasks[i].Number);
            }

            var numberWindow = new NumberInputWindow(TypeWindowInput.Task, _numbersTasks);
            numberWindow.ShowDialog();

            if (numberWindow.DialogResult == true)
            {
                _currentOptionEditor.CreateTask(numberWindow.Number);
                CreateTask(_currentOptionEditor.Option.Tasks[_currentOptionEditor.Option.Tasks.Count-1]);
            }
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

        private void CreateTask(LaboratoryWorkSystem.IViewTask tiedVariant)
        {
            int row = Grid.GetRow(GridAddTask);
            int column = Grid.GetColumn(GridAddTask);
            int place = (row * 6) + column + 1;

            Grid newTask = TaskCreate(column, row, tiedVariant);

            row = place / 6;
            column = place % 6;

            Grid.SetColumn(GridAddTask, column);
            Grid.SetRow(GridAddTask, row);
            MainTaskGrid.Children.Add(newTask);            

            if (place >= 18 && column == 0)
            {
                ScrollTasks.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                MainTaskGrid.RowDefinitions.Add(new RowDefinition());
                MainTaskGrid.Height += 198;
            }
        }

        private void ButtonChangeVariant(object sender, RoutedEventArgs e)
        {
            List<int> _numbersTask = new List<int>();

            for (int i = 0; i < _currentOptionEditor.Tasks.Count; i++)
            {
                _numbersTask.Add(_currentOptionEditor.Option.Tasks[i].Number);
            }

            LaboratoryWorkSystem.IViewTask tiedTask = (sender as Button).Tag as LaboratoryWorkSystem.IViewTask;

            var numberWindow = new NumberInputWindow(TypeWindowInput.Variant, _numbersTask);            
            numberWindow.ShowDialog();

            if (numberWindow.DialogResult == true)
            {
                _currentOptionEditor.UpdateNumberTask(tiedTask, numberWindow.Number);

                AddTaskWindow addTaskWindow = new AddTaskWindow(_currentLabEditor, _currentOptionEditor);
                addTaskWindow.Top = this.Top;
                addTaskWindow.Left = this.Left;
                addTaskWindow.Show();
                Close();
            }
        }

        private async void ButtonSaveData(object sender, RoutedEventArgs e)
        {
            await _currentLabEditor.SaveLabWork();
            //SaveButton.IsEnabled = false;
        }

        private Grid TaskCreate(int column, int row, LaboratoryWorkSystem.IViewTask tiedtask)
        {
            Grid task = new Grid();

            task.HorizontalAlignment = HorizontalAlignment.Left;
            task.Height = 177;
            task.VerticalAlignment = VerticalAlignment.Top;
            task.Width = 152;
            task.Background = GridVarTemp.Background;

            Thickness margin = new Thickness();

            margin = task.Margin;
            margin.Left = 10;
            margin.Top = 10;
            task.Margin = margin;

            {
                TextBlock textBlockTask = new TextBlock();

                textBlockTask.Text = "Задание " + tiedtask.Number;
                textBlockTask.HorizontalAlignment = HorizontalAlignment.Left;

                margin = textBlockTask.Margin;
                margin.Left = 0;
                margin.Top = 136;
                margin.Bottom = 19;

                textBlockTask.Margin = margin;
                textBlockTask.Height = 22;
                textBlockTask.VerticalAlignment = VerticalAlignment.Top;
                textBlockTask.FontSize = 14;
                textBlockTask.Width = 152;
                textBlockTask.TextAlignment = TextAlignment.Center;
                textBlockTask.FontFamily = (GridVarTemp.Children[0] as TextBlock).FontFamily;

                task.Children.Add(textBlockTask);
            }

            {
                Button button = new Button();

                button.Tag = tiedtask;
                button.Content = ((row * 6) + column).ToString();
                button.HorizontalAlignment = HorizontalAlignment.Left;

                button.Width = 152;
                button.Height = 177;
                button.VerticalAlignment = VerticalAlignment.Top;

                button.Click += ButtonTask;

                button.Background = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                button.BorderBrush = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                button.Foreground = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));

                Grid.SetRow(button, row);
                Grid.SetColumn(button, column);

                task.Children.Add(button);
            }

            // Delete button
            {
                Button buttonDelete = new Button();

                buttonDelete.Tag = tiedtask;
                buttonDelete.Content = ((row * 6) + column).ToString();
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

                buttonDelete.Click += ButtonDelete;

                buttonDelete.Background = (GridVarTemp.Children[2] as Button).Background;
                buttonDelete.BorderBrush = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                buttonDelete.Foreground = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));

                Grid.SetRow(buttonDelete, row);
                Grid.SetColumn(buttonDelete, column);

                task.Children.Add(buttonDelete);
            }

            //change button
            {
                Button changeButton = new Button();

                changeButton.Tag = tiedtask;
                changeButton.Content = ((row * 6) + column).ToString();
                changeButton.HorizontalAlignment = HorizontalAlignment.Left;

                margin = changeButton.Margin;
                margin.Left = 110;
                margin.Top = 155;
                margin.Right = 27;
                margin.Bottom = 7;

                changeButton.Margin = margin;
                changeButton.Width = 15;
                changeButton.Height = 15;
                changeButton.VerticalAlignment = VerticalAlignment.Top;

                changeButton.Click += ButtonChangeVariant;

                changeButton.Background = (GridVarTemp.Children[3] as Button).Background;
                changeButton.BorderBrush = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));
                changeButton.Foreground = new SolidColorBrush(Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF));

                Grid.SetRow(changeButton, row);
                Grid.SetColumn(changeButton, column);

                task.Children.Add(changeButton);                
            }

            Grid.SetRow(task, row);
            Grid.SetColumn(task, column);

            return task;
        }

        private void TaskDelete(int deletingTaskNum)
        {
            int place;

            int row = Grid.GetRow(GridAddTask);
            int column = Grid.GetColumn(GridAddTask);
            place = (row * 6) + column - 1;

            row = place / 6;
            column = place % 6;

            for (int i = deletingTaskNum + 2; i <= _currentOptionEditor.Option.Tasks.Count; i++)
            {
                int rowTask = Grid.GetRow(MainTaskGrid.Children[i]);
                int columnTask = Grid.GetColumn(MainTaskGrid.Children[i]);

                int varNumber = Convert.ToInt32(((MainTaskGrid.Children[i] as Grid).Children[2] as Button).Content);
                varNumber--;

                ((MainTaskGrid.Children[i] as Grid).Children[2] as Button).Content = varNumber.ToString();

                int taskplace = (rowTask * 6) + columnTask - 1;

                rowTask = taskplace / 6;
                columnTask = taskplace % 6;

                Grid.SetColumn(MainTaskGrid.Children[i], columnTask);
                Grid.SetRow(MainTaskGrid.Children[i], rowTask);
            }

            MainTaskGrid.Children.Remove(MainTaskGrid.Children[deletingTaskNum + 1]);

            if (place >= 17 && column == 5)
            {
                MainTaskGrid.RowDefinitions.Remove(MainTaskGrid.RowDefinitions[MainTaskGrid.RowDefinitions.Count - 1]);
                MainTaskGrid.Height -= 198;

                if (place < 18)
                {
                    ScrollTasks.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
            }

            Grid.SetColumn(GridAddTask, column);
            Grid.SetRow(GridAddTask, row);
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
    }
}
