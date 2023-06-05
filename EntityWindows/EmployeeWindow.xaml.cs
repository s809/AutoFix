using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace AutoFix
{
    /// <summary>
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : EntityWindow<Employee>
    {
        public EmployeeWindow(Employee? employee = null) : base(employee)
        {
            InitializeComponent();
            passwordBox.Password = _entity.Password;
            Closing += Window_Closing;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _entity.Password = ((PasswordBox)sender).Password;
        }

        private void Window_Closing(object? sender, CancelEventArgs e)
        {
            if (App.LoggedInEmployee == null)
            {
                if (DialogResult == true)
                {
                    App.LoggedInEmployee = _entity;
                }
                else
                {
                    var result = MessageBox.Show("При выходе без создания сотрудника приложение будет закрыто.", "Выйти без создания сотрудника?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result != MessageBoxResult.Yes)
                        e.Cancel = true;
                }
            }
        }
    }
}
