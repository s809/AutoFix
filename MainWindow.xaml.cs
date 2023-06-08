using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
                if (new EmployeeWindow(new Employee() { Position = EmployeePosition.Administrator }).ShowDialog() != true)
                {
                    Close();
                    return;
                }
            }

            DataContext = App.LoggedInEmployee;
            foreach (var tabControl in new[] { tabsRoot, tabsRepair, tabsWarehouse })
                tabControl.SelectedIndex = tabControl.Items.Cast<TabItem>().TakeWhile(i => i.Visibility != Visibility.Visible).Count();

            Refresh();
        }

        private void Refresh()
        {
            Title = $"{Resources["title"]} ({App.LoggedInEmployee!.FullName})";

            lvEmployees.ItemsSource = AppDbContext.GetAllEmployeesWithLeft();
            lvWarehouseItems.ItemsSource = AppDbContext.GetAllWarehouseItems();
            lvWarehouseProviders.ItemsSource = AppDbContext.GetAllWarehouseProviders();
            lvRepairOrders.ItemsSource = AppDbContext.GetAllRepairOrders();
            lvServices.ItemsSource = AppDbContext.GetAllServices();

            (filterOrdersByEmployeeBox_list.Collection, filterOrdersByEmployeeBox.SelectedIndex) = AppDbContext.GetAllMasters().WithSelectedIndex((filterOrdersByEmployeeBox.SelectedItem as Employee)?.Id, 1);
            FilterEmployees();
            FilterRepairOrders();
        }

        private void ShowAddOrEditDialog(Window window)
        {
            if (window.ShowDialog() == true)
                Refresh();
        }

        private void AddRepairOrder_Click(object sender, RoutedEventArgs e) => ShowAddOrEditDialog(new RepairOrderWindow(new RepairOrder { MasterId = App.LoggedInEmployee!.Id }));
        private void AddWarehouseItem_Click(object sender, RoutedEventArgs e) => ShowAddOrEditDialog(new WarehouseItemWindow());
        private void AddWarehouseProvider_Click(object sender, RoutedEventArgs e) => ShowAddOrEditDialog(new WarehouseProviderWindow());
        private void AddEmployee_Click(object sender, RoutedEventArgs e) => ShowAddOrEditDialog(new EmployeeWindow());
        private void AddService_Click(object sender, RoutedEventArgs e) => ShowAddOrEditDialog(new ServiceWindow());

        private void lvRepairOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e) => ShowAddOrEditDialog(new RepairOrderWindow(Entity.Clone<RepairOrder>(lvRepairOrders.SelectedItem)));
        private void lvWarehouse_MouseDoubleClick(object sender, MouseButtonEventArgs e) => ShowAddOrEditDialog(new WarehouseItemWindow(Entity.Clone<WarehouseItem>(lvWarehouseItems.SelectedItem)));
        private void lvProviders_MouseDoubleClick(object sender, MouseButtonEventArgs e) => ShowAddOrEditDialog(new WarehouseProviderWindow(Entity.Clone<WarehouseProvider>(lvWarehouseProviders.SelectedItem)));
        private void lvEmployees_MouseDoubleClick(object sender, MouseButtonEventArgs e) => ShowAddOrEditDialog(new EmployeeWindow(Entity.Clone<Employee>(lvEmployees.SelectedItem)));
        private void lvServices_MouseDoubleClick(object sender, MouseButtonEventArgs e) => ShowAddOrEditDialog(new ServiceWindow(Entity.Clone<Service>(lvServices.SelectedItem)));

        private void FilterEmployees()
        {
            var view = CollectionViewSource.GetDefaultView(lvEmployees.ItemsSource);
            view.Filter = showLeftEmployeesBox.IsChecked == true
                ? null
                : item => ((Employee)item).EndDate == null;
        }
        private void ShowLeftEmployeesBox_Checked(object sender, RoutedEventArgs e) => FilterEmployees();
        private void ShowLeftEmployeesBox_Unchecked(object sender, RoutedEventArgs e) => FilterEmployees();

        private void FilterRepairOrders()
        {
            if (lvRepairOrders == null) return;

            var view = CollectionViewSource.GetDefaultView(lvRepairOrders.ItemsSource);
            view.Filter = item =>
            {
                var allowedFinishDate = showFinishedOrdersBox.IsChecked == true;
                allowedFinishDate |= ((RepairOrder)item).FinishDate == null;

                var allowedEmployee = filterOrdersByEmployeeBox.SelectedItem is not Employee;
                if (!allowedEmployee)
                    allowedEmployee |= ((RepairOrder)item).MasterId == ((Employee)filterOrdersByEmployeeBox.SelectedItem).Id;

                if (!App.LoggedInEmployee!.HasPosition(EmployeePosition.ServiceManager, EmployeePosition.Cashier, EmployeePosition.Accountant))
                    allowedEmployee &= ((RepairOrder)item).MasterId == App.LoggedInEmployee.Id;

                return allowedFinishDate && allowedEmployee;
            };
        }
        private void ShowFinishedOrdersBox_Checked(object sender, RoutedEventArgs e) => FilterRepairOrders();
        private void ShowFinishedOrdersBox_Unchecked(object sender, RoutedEventArgs e) => FilterRepairOrders();
        private void filterOrdersByEmployeeBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => FilterRepairOrders();
    }
}
