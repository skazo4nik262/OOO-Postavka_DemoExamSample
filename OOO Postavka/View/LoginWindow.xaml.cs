using Microsoft.EntityFrameworkCore;
using OOO_Postavka.Models;
using OOO_Postavka.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OOO_Postavka
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, INotifyPropertyChanged
    {
        private DeBdContext _db;
        private const int MAX_FAILED_ATTEMPTS = 3;
        private int failedAttempts = 0;
        private int currentImageId;
        private ImageSource? currentImageSource;
        private int[] targetOrder = new int[4];
        private int[] correctOrder = { 1, 2, 3, 4 };
        private string login;
        private string password;

        public event PropertyChangedEventHandler? PropertyChanged;
        public string Login { get => login; set { login = value; Signal(); } }
        public string Password { get => password; set { password = value; Signal(); } }

        public LoginWindow()
        {
            InitializeComponent();
            DataContext = this;
            _db = DeBdContext.Instance;
            RandomSourceImages();
        }

        void Signal([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        private void RandomSourceImages()
        {
            var buttons = sourcePanel.Children.OfType<Button>().ToArray();
            var random = new Random();
            for (int i = buttons.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (buttons[i].Content, buttons[j].Content) = (buttons[j].Content, buttons[i].Content);
                (buttons[i].Tag, buttons[j].Tag) = (buttons[j].Tag, buttons[i].Tag);
            }
        }
        private void GetNewImageClick(object sender, RoutedEventArgs e)
        {
            if (currentImageId == 0 || currentImageSource == null)
            {
                MessageBox.Show("Выберите картинку");
                return;
            }
            if (sender is Button btn)
            {
                int index;
                if (btn.Name == "btn1") index = 0;
                else if (btn.Name == "btn2") index = 1;
                else if (btn.Name == "btn3") index = 2;
                else index = 3;
                targetOrder[index] = currentImageId;
                ((Image)btn.Content).Source = currentImageSource;
                currentImageId = 0;
                currentImageSource = null;
            }
        }
        private void PickImageClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                currentImageSource = (btn.Content as Image)?.Source;
                currentImageId = int.TryParse(btn.Tag.ToString(), out var id) ? id : 0;
            }
        }

        private void OpenPhoneValidationClick(object sender, RoutedEventArgs e)
        {
            new PhoneValidationWindow().Show();
            Close();
        }
        private void OpenRegWindowClick(object sender, RoutedEventArgs e)
        {
            new RegistrationWindow().Show();
            Close();
        }

        private async void LoginClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Login))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            var login = Login.Trim();
            var user = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(s => s.Login == login);

            if (user == null)
            {
                MessageBox.Show("Вы ввели неверный логин или пароль. Пожалуйста проверьте ещё раз введенные данные");
                ResetCaptcha();
                return;
            }

            if (user.IsBlocked == true)
            {
                MessageBox.Show("Вы заблокированы. Обратитесь к администратору");
                return;
            }

            if (!VerifyCaptcha())
            {
                failedAttempts++;
                if (failedAttempts >= MAX_FAILED_ATTEMPTS)
                {
                    user.IsBlocked = true;
                    await _db.SaveChangesAsync();
                    MessageBox.Show("Пазл собран неверно. Ваша учетная запись заблокирована. Обратитесь к администратору");
                }
                else
                {
                    MessageBox.Show("Пазл собран неверно. Попробуйте снова.");
                }
                ResetCaptcha();
                return;
            }

            if (user.Password != Password)
            {
                failedAttempts++;
                if (failedAttempts >= MAX_FAILED_ATTEMPTS)
                {
                    user.IsBlocked = true;
                    await _db.SaveChangesAsync();
                    MessageBox.Show("Вы ввели неверный логин или пароль. Ваша учетная запись заблокирована. Обратитесь к администратору");
                    return;
                }
                MessageBox.Show("Вы ввели неверный логин или пароль. Пожалуйста проверьте ещё раз введенные данные");
                ResetCaptcha();
                return;
            }

            failedAttempts = 0;
            MessageBox.Show("Вы успешно авторизовались");

            if (user.Role?.Title == "Администратор")
            {
                new AdminWindow().Show();
                Close();
            }
        }

        private void ResetCaptcha()
        {
            targetOrder = new int[4];
            foreach (Button btn in targetGrid.Children)
                ((Image)btn.Content).Source = null;
            RandomSourceImages();
            currentImageSource = null;
            currentImageId = 0;
        }

        private bool VerifyCaptcha()
        {
            for (int i = 0; i < 4; i++)
            {
                if (targetOrder[i] != correctOrder[i])
                    return false;
            }
            return true;
        }
    }
}
