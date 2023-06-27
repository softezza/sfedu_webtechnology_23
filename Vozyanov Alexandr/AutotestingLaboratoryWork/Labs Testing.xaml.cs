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

using System.IO;
using ProgramValidation;

namespace Autotesting
{
    /// <summary>
    /// Логика взаимодействия для Labs_Testing.xaml
    /// </summary>
    public partial class Labs_Testing : Window
    {
        private Color taskChoice = Color.FromArgb(0xFF, 0xD8, 0xD8, 0xD8);
        private Color taskComplite = Color.FromArgb(0xFF, 0xD8, 0xFF, 0xD8);
        private Color transparentsyArrow = Color.FromArgb(0x33, 0x00, 0x00, 0x00);

        private string codeText = "";
        private const string codeGeneric = "#include <iostream>\n\n"
                                + "using namespace std;\n\n"
                                + "int main()\n"
                                + "{\n"
                                + "    cout << \"Hello World\";\n\n"
                                + "    return 0;\n"
                                + "}";

        private List<Label> tasksLable;
        private List<LaboratoryWorkSystem.IViewTask> tasks;
        private Dictionary<LaboratoryWorkSystem.IViewTask,bool> _currentTasksComplete;
        private List<string> taskscode;
        private int currentTask = 0;
        private int currentLabNum;
        private string pathExeFile = string.Empty;

        private Labs_Testing()
        {
            InitializeComponent();
        }

        public Labs_Testing(LaboratoryWorkSystem.IViewOption variant, int currentLab)
        {
            InitializeComponent();

            taskscode  = new List<string>();
            tasksLable = new List<Label>();

            this.tasks = variant.Tasks.ToList();

            _currentTasksComplete = CashData.IsCompleteTasks[CashData.labsLW[currentLab]][variant];

            codeText = textCode.Text;
            textCode.Text = codeGeneric;
            VarLabel.Content = "Вариант " + variant.Number;
            currentLabNum = currentLab;

            for (int i = 0; i < tasks.Count; i++)
            {
                tasksLable.Add(CreateTask(i,tasks[i].Number));

                if (i >= 10)
                {
                    GridTasks.RowDefinitions.Add(new RowDefinition());
                    GridTasks.Height += 58;
                }
            }

            ResetColorTasks();

            if (tasks.Count > 0)
            {
                TaskFront();

                for (int i = 0; i < tasks.Count; i++)
                {
                    taskscode.Add(codeGeneric);
                }

                tasksLable[0].Background = new SolidColorBrush(taskChoice);
                taskText.Text = tasks[currentTask].Description;

                if (tasks.Count >= 10)
                {
                    ImageScrollDown.OpacityMask = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00));
                }
            }
            else
            {
                buttonCheck.IsEnabled = false;
                outputDataLabel.Content = "";
                inputDataLabel.Content = "";
                taskText.Text = "";
            }
        }

        private Label CreateTask(int row, int taskNumber)
        {
            Label task = new Label();
            Grid.SetRow(task, row);

            task.MouseDown += TaskClick;
            task.Tag = row;
            task.Content = "Задание " + taskNumber;
            task.HorizontalContentAlignment = HorizontalAlignment.Center;
            task.VerticalContentAlignment = VerticalAlignment.Center;
            task.Width = 194;
            task.Height = 58;
            task.FontSize = 24;
            task.Cursor = Cursors.Hand;
            task.FontFamily = taskTemp.FontFamily;
            task.Background = new SolidColorBrush(Colors.Transparent);
           
            GridTasks.Children.Add(task);

            return task;
        }

        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {   
            CheckLab labsCheckWindow = new CheckLab(TypeValidation.Code, textCode.Text, tasks[currentTask]);

            darkest.Visibility = Visibility.Visible;

            if (labsCheckWindow.ShowDialog().Value) { }

            _currentTasksComplete[tasks[currentTask]] = labsCheckWindow.IsComplete;

            darkest.Visibility = Visibility.Hidden;

            if (labsCheckWindow.ErrorText != "")
            {
                taskText.Text = labsCheckWindow.ErrorText;
            }

            if (_currentTasksComplete[tasks[currentTask]])
            {
                buttonCheck.IsEnabled = false;
                tasksLable[currentTask].Background = new SolidColorBrush(taskComplite);
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            ChoiseVariantLabWindow varListWindow = new ChoiseVariantLabWindow(CashData.labsLW[currentLabNum], currentLabNum);
            varListWindow.Top = Top;
            varListWindow.Left = Left;            
            varListWindow.Show();

            Close();
        }

        private void ButtonExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TaskClick(object sender, MouseButtonEventArgs e)
        {
            ResetColorTasks();
            taskscode[currentTask] = textCode.Text; 
            Label label = sender as Label;
            label.Background = new SolidColorBrush(taskChoice);
            currentTask = Convert.ToInt32(label.Tag.ToString());
            textCode.Text = taskscode[currentTask];

            buttonCheck.IsEnabled = !_currentTasksComplete[tasks[currentTask]];
            
            TaskLabel.Content = label.Content;

            TaskFront();

            taskText.Text = tasks[currentTask].Description;
        }

        private void TaskFront()
        {
            List<DataSet> dataSet = tasks[currentTask].GetDataSets();

            if (dataSet.Count > 0)
            {
                inputDataLabel.Content = "Входные данные:";
                foreach (var data in dataSet[0].InputData)
                {
                    inputDataLabel.Content += "\n" + data.Value.ToString();
                }

                outputDataLabel.Content = "Выходные данные:";

                foreach (var data in tasks[currentTask].GetDataSets()[0].ExpectedOutputData)
                {
                    outputDataLabel.Content += "\n" + data.Value.ToString();
                }
            }
            else
            {
                inputDataLabel.Content = " ";
                outputDataLabel.Content = " ";
                buttonCheck.IsEnabled = false;
            }
        }
        
        private void ResetColorTasks()
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (_currentTasksComplete[tasks[i]]) 
                {
                    tasksLable[i].Background = new SolidColorBrush(taskComplite);
                }
                else
                {
                    tasksLable[i].Background = new SolidColorBrush(Colors.Transparent);
                }
            }            
        }

        private void textCode_Drop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];

            if (files != null && files.Any())
            {
                if (FileHandler(files.First()))
                {
                    CheckLab labsCheckWindow = new CheckLab(TypeValidation.Exe, files.First(), tasks[currentTask]);

                    darkest.Visibility = Visibility.Visible;

                    if (labsCheckWindow.ShowDialog().Value) { }

                    _currentTasksComplete[tasks[currentTask]] = labsCheckWindow.IsComplete;

                    darkest.Visibility = Visibility.Hidden;

                    if (labsCheckWindow.ErrorText != "")
                    {
                        taskText.Text = labsCheckWindow.ErrorText;
                    }

                    if (_currentTasksComplete[tasks[currentTask]])
                    {
                        buttonCheck.IsEnabled = false;
                        tasksLable[currentTask].Background = new SolidColorBrush(taskComplite);
                    }
                }
            }
        }

        private bool FileHandler(string fileName)
        {
            fileName = Path.GetFileName(fileName);

            if (Path.GetExtension(fileName) != ".exe")
            {
                return false;
            }

            return true;
        }

        private void textCode_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void ScrollTasks_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if(tasks.Count >= 10)
            {
                if(ScrollTasks.VerticalOffset > 0)
                {
                    ImageScrollUp.OpacityMask = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    ImageScrollUp.OpacityMask = new SolidColorBrush(Color.FromArgb(0x33, 0x00, 0x00, 0x00));
                }

                if (ScrollTasks.VerticalOffset < ScrollTasks.ScrollableHeight)
                {
                    ImageScrollDown.OpacityMask = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    ImageScrollDown.OpacityMask = new SolidColorBrush(Color.FromArgb(0x33, 0x00, 0x00, 0x00));
                }
            }
        }
    }
}
