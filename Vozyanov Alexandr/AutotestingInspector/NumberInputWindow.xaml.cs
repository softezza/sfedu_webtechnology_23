using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace AutotestingInspector
{
    public enum TypeWindowInput
    {
        Variant = 0,
        Task = 1
    }

    /// <summary>
    /// Логика взаимодействия для NumberInputWindow.xaml
    /// </summary>
    public partial class NumberInputWindow : Window
    {
        private List<int> _numbersExist;

        public int Number { get; private set; }

        private NumberInputWindow()
        {
            InitializeComponent();
        }

        public NumberInputWindow(TypeWindowInput type, List<int> numbersExist)
        {
            Top = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 3;
            Left = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 3;
            InitializeComponent();

            switch(type)
            {
                case TypeWindowInput.Variant:
                    TextInput.Content = "Введите номер варианта";
                    break;
                case TypeWindowInput.Task:
                    TextInput.Content = "Введите номер задания";
                    break;
            }

            _numbersExist = numbersExist;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
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

        private void Saving()
        {
            if (NumberBox.Text != "")
            {
                try
                {
                    Number = Convert.ToInt32(NumberBox.Text);
                }
                catch
                {
                    CashData.Alert("Номер имеет неверный тип.");
                    return;
                }

                foreach (var num in _numbersExist)
                {
                    if (num == Number)
                    {
                        CashData.Alert("Такой номер уже есть.");
                        return;
                    }
                }

                if(Number <= 0)
                {
                    CashData.Alert("Номер не может быть меньше единицы.");
                    return;
                }

                DialogResult = true;
                Close();
            }
            else
            {
                CashData.Alert("Номер не задан.");
            }
        }       
    }
}
