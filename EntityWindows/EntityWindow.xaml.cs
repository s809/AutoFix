using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutoFix
{
    /// <summary>
    /// Логика взаимодействия для EntityWindow.xaml
    /// </summary>
    public partial class EntityWindow<T> : Window where T : Entity, new()
    {
        protected readonly T _entity;

        public EntityWindow() : this(null) { }

        public EntityWindow(T? entity)
        {
            _entity = entity ?? new();
            DataContext = _entity;
        }

        protected void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_entity.Save())
                DialogResult = true;
        }

        protected void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_entity.Id == 0)
            {
                MessageBox.Show("Невозможно удалить без создания.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Вы уверены?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (_entity.Delete())
                    DialogResult = true;
            }
        }
    }
}
