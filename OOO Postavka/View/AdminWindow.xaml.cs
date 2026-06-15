using Microsoft.EntityFrameworkCore;
using OOO_Postavka.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window, INotifyPropertyChanged
    {
        private DeBdContext _db;
        private ObservableCollection<User>? usersList;

        public ObservableCollection<User>? UsersList { get => usersList; set { usersList = value; Signal(); } }
        public User? SelectedUser { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<Role> roles;

        public AdminWindow()
        {
            InitializeComponent();
            DataContext = this;
            _db = DeBdContext.Instance;
            Loaded += async (a, b) => await ListUsersInitializeAsync();
        }

        void Signal([CallerMemberName] string? prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        private async Task ListUsersInitializeAsync()
        {
            UsersList?.Clear();
            UsersList = new ObservableCollection<User>(await _db.Users.Include(u => u.Role).ToListAsync());
            roles = new ObservableCollection<Role>(await _db.Roles.ToListAsync());
        }

        private async void AddUserClick(object sender, RoutedEventArgs e)
        {
            var dialog = new AddEditUserWindow() { Title = "Добавление пользователя", VisibilityContent = Visibility.Collapsed, Roles=roles };
            if (dialog.ShowDialog() == true)
            {
                var login = dialog.Login;
                var password = dialog.Password;
                var role = dialog.Role;
                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Заполните все поля");
                    return;
                }
                if (await _db.Users.AnyAsync(u => u.Login == login))
                {
                    MessageBox.Show("Пользователь уже существует");
                    return;
                }
                await _db.Users.AddAsync(new User
                {
                    Login = login,
                    Password = password,
                    RoleId = role.Id,
                    IsBlocked = false
                });
                await _db.SaveChangesAsync();
                await ListUsersInitializeAsync();
            }
        }

        private async void EditUserClick(object sender, RoutedEventArgs e)
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("Выберите пользователя");
                return;
            }
            var dialog = new AddEditUserWindow()
            {
                Title = "редактирование пользователя",
                Login = SelectedUser.Login,
                IsBlocked = SelectedUser.IsBlocked == true,
                VisibilityContent = Visibility.Visible,
                Role = SelectedUser.Role,
                Roles = roles,
                Password = SelectedUser.Password,
                PasswordControl = SelectedUser.Password,

            };
            if (dialog.ShowDialog() == true)
            {
                var user = await _db.Users.FirstOrDefaultAsync(u=>u.Id==SelectedUser.Id);
                user.Password = dialog.Password;
                user.IsBlocked = dialog.IsBlocked;
                user.RoleId = dialog.Role.Id;
                await _db.SaveChangesAsync();
                await ListUsersInitializeAsync();
            }
        }

        private async void UnblockUserClick(object sender, RoutedEventArgs e)
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("Выберите пользователя");
                return;
            }
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == SelectedUser.Id);
            user.IsBlocked = false;
            await _db.SaveChangesAsync();
            await ListUsersInitializeAsync();
            MessageBox.Show("Пользователь разблокирован");
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }
    }
}
