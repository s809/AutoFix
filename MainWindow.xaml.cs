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

            if (AppDbContext.CountEmployees() > 0)
            {
                if (new LoginWindow().ShowDialog() != true)
                {
                    Close();
                    return;
                }
            }
            else
            {
                MessageBox.Show("При первом запуске необходимо создать учетную запись сотрудника.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (new EmployeeWindow(new Employee()).ShowDialog() != true)
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

            lvEmployees.ItemsSource = AppDbContext.GetAllEmployees();
            lvWarehouseItems.ItemsSource = AppDbContext.GetAllWarehouseItems();
            lvWarehouseProviders.ItemsSource = AppDbContext.GetAllWarehouseProviders();
            lvRepairOrders.ItemsSource = AppDbContext.GetAllRepairOrders();
            lvServices.ItemsSource = AppDbContext.GetAllServices();
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
        private void AddService_Click(object sender, RoutedEventArgs e) => ShowAddOrEditDialog(new ServiceWindow());

        private void lvRepairOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e) => ShowAddOrEditDialog(new RepairOrderWindow(Entity.Clone<RepairOrder>(lvRepairOrders.SelectedItem)));
        private void lvWarehouse_MouseDoubleClick(object sender, MouseButtonEventArgs e) => ShowAddOrEditDialog(new WarehouseItemWindow(Entity.Clone<WarehouseItem>(lvWarehouseItems.SelectedItem)));
        private void lvProviders_MouseDoubleClick(object sender, MouseButtonEventArgs e) => ShowAddOrEditDialog(new WarehouseProviderWindow(Entity.Clone<WarehouseProvider>(lvWarehouseProviders.SelectedItem)));
        private void lvEmployees_MouseDoubleClick(object sender, MouseButtonEventArgs e) => ShowAddOrEditDialog(new EmployeeWindow(Entity.Clone<Employee>(lvEmployees.SelectedItem)));
        private void lvServices_MouseDoubleClick(object sender, MouseButtonEventArgs e) => ShowAddOrEditDialog(new ServiceWindow(Entity.Clone<Service>(lvServices.SelectedItem)));
    }
}
