using KingIT.ViewModels;
using KingIT.Infrastructure;
using KingIT.Models;
using KingIT.ViewModels.Base;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using KingIT.Views.Pages;

namespace KingIT.ViewModels
{
    public class AutorizationPageViewModel : BaseViewModel
    {
        #region Логин

        private string _Login;
        public string Login
        {
            get => _Login;
            set => Set(ref _Login, value);
        }

        #endregion

        #region Пароль

        private string _Password;
        public string Password
        {
            get => _Password;
            set => Set(ref _Password, value);
        }

        #endregion

        #region Кол-во оставшихся попыток без Каптчи

        private int _CountOfPoints;
        public int CountOfPoints
        {
            get => _CountOfPoints;
            set => Set(ref _CountOfPoints, value);
        }

        #endregion

        #region Сообщение об ошибках

        private string _Message;
        public string Message
        {
            get => _Message;
            set => Set(ref _Message, value);
        }

        #endregion

        #region Каптча

        #region Текст Каптчи Пользовательский

        private string _CaptchaUserText;
        public string CaptchaUserText
        {
            get => _CaptchaUserText;
            set => Set(ref _CaptchaUserText, value);
        }

        #endregion

        #region Видимость каптчи

        private System.Windows.Visibility _CaptchaVisibility;
        public System.Windows.Visibility CaptchaVisibility
        {
            get => _CaptchaVisibility;
            set => Set(ref _CaptchaVisibility, value);
        }

        #endregion

        #region Фотка и текст капчи

        public System.Windows.Controls.Image CaptchaImage { get; set; }
        private string CaptchaText;

        #endregion

        #region Методы создания Каптчи

        public Bitmap CreateImage(int Width, int Height)
        {
            Random rand = new Random();
            Bitmap result = new Bitmap(Width, Height);
            int xPos = rand.Next(0, Width - 120);
            int yPos = rand.Next(15, Height - 35);
            Brush[] colors =
            {
                Brushes.Black,
                Brushes.Red,
                Brushes.Blue,
                Brushes.Green
            };
            Graphics g = Graphics.FromImage(result);
            g.Clear(Color.Gray);

            CaptchaText = string.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 5; i++)
                CaptchaText += ALF[rand.Next(ALF.Length)];

            g.DrawString(CaptchaText,
                new Font("Arial", 20),
                colors[rand.Next(colors.Length)],
                new PointF(xPos, yPos));
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                    if (rand.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);

            return result;
        }

        private BitmapImage GenerateBitmapImage(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage ix = new BitmapImage();
            ix.BeginInit();
            ix.CacheOption = BitmapCacheOption.OnLoad;
            ix.StreamSource = ms;
            ix.EndInit();
            return ix;
        }

        #endregion

        #endregion

        public AutorizationPageViewModel()
        {
            Login = "Elizor@gmai.com";
            Password = "yntiRS";
            CountOfPoints = 3;
            CaptchaImage = new System.Windows.Controls.Image() { Width = 350, Height = 80 };
            CaptchaVisibility = System.Windows.Visibility.Hidden;
            AutorizateAccountCommand = new LambdaCommand(OnAutorizateAccountCommandExecuted, CanAutorizateAccountCommandExecute);
        }

        #region Команда попытки входа под логином и паролем

        public ICommand AutorizateAccountCommand { get; }
        private bool CanAutorizateAccountCommandExecute(object d) => true;
        private void OnAutorizateAccountCommandExecuted(object d)
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
                Message = "Введите логин и пароль";
            else
            {
                var Employee = KingITEntities.GetContext().Employees.Where(x => x.employeeLogin.ToLower() == Login.ToLower() && x.employeePassword == Password).FirstOrDefault();
                if ((CountOfPoints > 0 || CaptchaUserText.ToUpper() == CaptchaText) && Employee != null)
                {
                    Message = $"Вы вошли как user с фамилией: {Employee.employeeSurname}";
                    MainWindowViewModel.InsertedEmployee = Employee.employeeNumber;
                    if (Employee.employeeRole == "Менеджер С")
                        MainWindowViewModel._CurrentViewModel.CurrentPage = new ManagerCPage();
                    if (Employee.employeeRole == "Менеджер А")
                        MainWindowViewModel._CurrentViewModel.CurrentPage = new ManagerAChoosenPage();
                    if (Employee.employeeRole == "Администратор")
                        MainWindowViewModel._CurrentViewModel.CurrentPage = new AdministratorPage();
                }
                else if (Employee == null)
                    Message = "Неверный логин или пароль";
                else if (CaptchaText != CaptchaUserText.ToUpper())
                    Message = "Неверная CAPTCHA";
                else
                    Message = "Ошибка входа";

                if (CountOfPoints > -1)
                    CountOfPoints -= 1;
                if (CountOfPoints == 0)
                    CaptchaVisibility = System.Windows.Visibility.Visible;
                if (CountOfPoints < 1)
                    CaptchaImage.Source = GenerateBitmapImage(CreateImage(350, 80));
            }
        }

        #endregion

        #region Метод для проверки сложности пароля и вспомогательные методы проверки наличия символов для тестирования

        public static string IsGoodPassword(string passwod)
        {
            if (passwod.Length > 20 || passwod.Length < 8)
                return "Пароль должен иметь длину от 8 до 20 символов";
            if (!IsHaveNum(passwod))
                return "Пароль должен содержать в себя хотя бы одну цифру";
            if (!IsHaveSpecial(passwod))
                return "Пароль должен содержать в себе хотя бы один спецсимвол";
            if (!IsHaveOtherWise(passwod))
                return "Пароль должен иметь как прописные, так и строчные буквы";
            return "Пароль корректен";
        }

        private static bool IsHaveNum(string s)
        {
            char[] chars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            foreach (char c in chars)
                if (s.Contains(c))
                    return true;
            return false;
        }

        private static bool IsHaveSpecial(string s)
        {
            char[] chars = new char[] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-' };
            foreach (char c in chars)
                if (s.Contains(c))
                    return true;
            return false;
        }

        private static bool IsHaveOtherWise(string s)
        {
            bool HighB = false, LowB = false;
            foreach(var c in s)
            {
                if (char.IsLower(c)) LowB = true;
                if (char.IsUpper(c)) HighB = true;
                if (LowB && HighB) return true;
            }
            return false;
        }

        #endregion
    }
}
