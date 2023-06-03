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
using System.Windows.Shapes;

namespace AutoFix
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e) => Login();

        private void LoginWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Login();
        }

        private void Login()
        {
            App.LoggedInEmployee = AppDbContext.FindLoginEmployee(username.Text, password.Password);
            if (App.LoggedInEmployee != null)
                DialogResult = true;
            else
                MessageBox.Show("Неверное имя пользователя или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
