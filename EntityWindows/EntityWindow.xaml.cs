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
    }
}
