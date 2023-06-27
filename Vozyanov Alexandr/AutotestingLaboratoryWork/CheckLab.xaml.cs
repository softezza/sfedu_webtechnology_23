using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ProgramValidation;
using LaboratoryWorkSystem;

namespace Autotesting
{

    public enum TypeValidation
    {
        Code,
        Exe
    }

    /// <summary>
    /// Логика взаимодействия для CheckLab.xaml
    /// </summary>
    public partial class CheckLab : Window
    {
        private string _textDataPathOrCode = "";
        public string ErrorText { get; private set; } = "";

        private IViewTask _tasks;

        private TypeValidation _typeValidation;

        public bool IsComplete { get; private set; }

        private CheckLab()
        {
            InitializeComponent();
        }

        public CheckLab(TypeValidation typeValidation, string code, IViewTask task)
        {
            Top = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 3;
            Left = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 3;
            _typeValidation = typeValidation;
            _tasks = task;
            _textDataPathOrCode = code;
            InitializeComponent();
        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            bool result;

            try
            {
                switch (_typeValidation)
                {
                    case TypeValidation.Code:
                        result = await new Validator().ValidateCodeCpp(_textDataPathOrCode, _tasks.GetDataSets().ToArray());
                        break;
                    case TypeValidation.Exe:
                        result = await new Validator().ValidateExe(_textDataPathOrCode, _tasks.GetDataSets().ToArray());
                        break;
                    default:
                        result = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                result = false;
                ErrorText = ex.ToString();
            }

            if (result == true)
            {
                MessageFront("Правильно", @"Resources\Correct.png");
            }
            else
            {
                MessageFront("Не правильно", @"Resources\Error.png");
            }

            IsComplete = result;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MessageFront(string message, string imageSource)
        {
            checkLabel.Content = message;
            LoadIcon.Source = new BitmapImage(new Uri(imageSource, UriKind.Relative));
        }
    }
}