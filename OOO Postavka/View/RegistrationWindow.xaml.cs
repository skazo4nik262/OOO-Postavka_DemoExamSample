using Microsoft.EntityFrameworkCore;
using OOO_Postavka.Models;
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

namespace OOO_Postavka.View
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window, INotifyPropertyChanged
    {

        private DeBdContext _db;
        private string login;
        private string password;
        private string passwordControl;

        public event PropertyChangedEventHandler? PropertyChanged;
        public string Login { get => login; set { login = value; Signal(); } }
        public string Password { get => password; set { password = value; Signal();} }
        public string PasswordControl { get => passwordControl; set { passwordControl = value; Signal();} }
        public RegistrationWindow()
        {
            InitializeComponent();
            _db = DeBdContext.Instance;
        }
        
        
        void Signal([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        private async void RegistrationClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(PasswordControl) || string.IsNullOrWhiteSpace(Login))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            if (Password != PasswordControl)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }
            var existing = await _db.Users.FirstOrDefaultAsync(s => s.Login == Login);
            if (existing != null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует");
                return;
            }
            var userRole = await _db.Roles.FirstOrDefaultAsync(r => r.Title == "Пользователь");
            await _db.Users.AddAsync(new User { Login = Login, Password = Password, RoleId = userRole?.Id ?? 1 });
            await _db.SaveChangesAsync();
            MessageBox.Show("Регистрация успешна");
            Login = "";
            Password = "";
            PasswordControl = "";
        }

        private void OpenLoginWindowClick(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }
    }
}
