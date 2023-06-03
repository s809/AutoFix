using System.Windows;
using System.Windows.Input;

namespace AutoFix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (AppDbContext.CountEmployees() != 0)
            {
                if (new LoginWindow().ShowDialog() != true)
                {
                    Close();
                    return;
                }
            }
            else
            {
                MessageBox.Show("При первом запуске необходимо создать учетную запись директора.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (new EmployeeWindow(new Employee()
                {
                    Position = App.AdminPosition
                }).ShowDialog() != true)
                {
                    Close();
                    return;
                }
            }

            Refresh();
        }

        private void Refresh()
        {
            Title = $"{Resources["title"]} ({App.LoggedInEmployee!.Name})";

            lwEmployees.ItemsSource = AppDbContext.GetAllEmployees();
            lwWarehouseItems.ItemsSource = AppDbContext.GetAllWarehouseItems();
            lwWarehouseProviders.ItemsSource = AppDbContext.GetAllWarehouseProviders();
            lwRepairOrders.ItemsSource = AppDbContext.GetAllRepairOrders();
        }

        private void ShowAddOrEditDialog(Window window)
        {
            if (window.ShowDialog() == true)
                Refresh();
        }

        private void AddRepairOrder_Click(object sender, RoutedEventArgs e) => ShowAddOrEditDialog(new RepairOrderWindow());
        private void AddWarehouseItem_Click(object sender, RoutedEventArgs e) => ShowAddOrEditDialog(new WarehouseItemWindow());
        private void AddWarehouseProvider_Click(object sender, RoutedEventArgs e) => ShowAddOrEditDialog(new WarehouseProviderWindow());
        private void AddEmployee_Click(object sender, RoutedEventArgs e) => ShowAddOrEditDialog(new EmployeeWindow());

        private void lwRepairOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e) => ShowAddOrEditDialog(new RepairOrderWindow(Entity.Clone<RepairOrder>(lwRepairOrders.SelectedItem)));
        private void lwWarehouse_MouseDoubleClick(object sender, MouseButtonEventArgs e) => ShowAddOrEditDialog(new WarehouseItemWindow(Entity.Clone<WarehouseItem>(lwWarehouseItems.SelectedItem)));
        private void lwProviders_MouseDoubleClick(object sender, MouseButtonEventArgs e) => ShowAddOrEditDialog(new WarehouseProviderWindow(Entity.Clone<WarehouseProvider>(lwWarehouseProviders.SelectedItem)));
        private void lwEmployees_MouseDoubleClick(object sender, MouseButtonEventArgs e) => ShowAddOrEditDialog(new EmployeeWindow(Entity.Clone<Employee>(lwEmployees.SelectedItem)));
    }
}
