using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Autotesting
{
    /// <summary>
    /// Логика взаимодействия для формы авторизации
    /// </summary>
    public partial class MainWindow : Window
    {
        private const bool _isAuthorizathionEnabled = false;

        public MainWindow()
        {
            InitializeComponent();
            InitializeLabels();
        }

        private void InitializeLabels()
        {
            labelGroup.Content = "";
            labelNumberZach.Content = "";
            labelFIO.Content = "";
        }

        private string StringClearing(string text)
        {
            if (text.Length > 0)
            {
                while (text[text.Length - 1] == ' ')
                {
                    text = text.Remove(text.Length - 1, 1);
                }

                while (text[0] == ' ')
                {
                    text = text.Remove(0, 1);
                }
            }

            return text;
        }

        private void TextBoxZach_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = textBoxNumberZach.Text;

            text = StringClearing(text);

            if (text == "" || Regex.IsMatch(text, @"^[0-9]{6}$") || Regex.IsMatch(text, @"^[0-9]{7}$"))
            {
                labelNumberZach.Content = "";

                if(text != "")
                {
                    CashData.numberZach = textBoxNumberZach.Text;
                }
            }
            else
            {
                CashData.numberZach = "";
                labelNumberZach.Content = "* Неверный формат номера зачётки";
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void TextBoxFIO_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = textBoxFIO.Text;

            text = StringClearing(text);

            if (text == "" || Regex.IsMatch(text, @"^[А-Я][а-я]+\s[А-Я][а-я]+\s[А-Я][а-я]+$") || Regex.IsMatch(text, @"^[А-Я][а-я]+\s[А-Я].\s[А-Я].$") || Regex.IsMatch(text, @"^[А-Я][а-я]+\s[А-Я][а-я]+$") || Regex.IsMatch(text, @"^[А-Я][а-я]+\s[А-Я].$"))
            {
                labelFIO.Content = "";

                if (text != "")
                {
                    CashData.FIO = textBoxFIO.Text;
                }
            }
            else
            {
                CashData.FIO = "";
                labelFIO.Content = "* Неверное указание ФИО";
            }
        }

        private void TextBoxGroup_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = textBoxGroup.Text;

            text = StringClearing(text);

            if (text == "" || Regex.IsMatch(text, @"^[А-Я]{3}-[0-5]-[0-9]{3}$"))
            {
                labelGroup.Content = "";

                if (text != "")
                {
                    CashData.group = textBoxGroup                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   .Text;
                }
            }
            else
            {
                CashData.group = "";
                labelGroup.Content = "* Неверный формат группы";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_isAuthorizathionEnabled == true)
            {
                if (CashData.FIO != "" && CashData.group != "" && CashData.numberZach != "")
                {
                    LabsBase labsListWindow = new LabsBase();
                    labsListWindow.Show();
                    Close();
                    //TODO: Autorization
                }
            }
            else
            {
                LabsBase labsListWindow = new LabsBase();
                labsListWindow.Top = Top;
                labsListWindow.Left = Left;
                labsListWindow.Show();
                Close();
            }           
        }

        private void ButtonExit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}