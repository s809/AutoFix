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

        protected void NumberBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9.]+").IsMatch(e.Text);
        }

        protected void NumberBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            var text = (string)e.DataObject.GetData(typeof(string));
            text = new Regex("[^0-9.]+").Replace(text, "");
            e.CancelCommand();
            ((TextBox)sender).Text = text;
        }
    }
}
