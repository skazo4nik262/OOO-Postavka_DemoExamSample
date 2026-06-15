using OOO_Postavka.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для AddEditUserWindow.xaml
    /// </summary>
    public partial class AddEditUserWindow : Window
    {
        public string Login {  get; set; }
        public string Password { get; set; }
        public string PasswordControl { get; set; }
        public bool IsBlocked {  get; set; }
        public Role Role {  get; set; }
        public ObservableCollection<Role> Roles { get; set; }
        public Visibility VisibilityContent { get; set; }

        public AddEditUserWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            if (Password != PasswordControl)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }
            DialogResult = true;
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
}
