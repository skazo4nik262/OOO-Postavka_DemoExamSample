using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для PhoneValidationWindow.xaml
    /// </summary>
    public partial class PhoneValidationWindow : Window
    {
        private const string API_URL = "http://localhost:4444/TransferSimulator/mobilePhone";
        private Phone ApiResult {  get; set; }
        public PhoneValidationWindow()
        {
            InitializeComponent();
        }

        private async void GetDataClick(object sender, RoutedEventArgs e)
        {
            ApiResult = await API_URL.GetJsonAsync<Phone>();
            phoneLabel.Content = ApiResult.Value;
        }
        record Phone {public string Value { get; set; } }

        private void PushTestResultClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ApiResult.Value))
            {
                string pattern = @"^\+7 \d{3} \d{3}-\d{2}-\d{2}$";
                if (Regex.IsMatch(ApiResult.Value, pattern))
                    resultLabel.Content = "Корректный номер телефона";
                else resultLabel.Content = "Не корректный номер телефона";
            }
        }

    }
}
