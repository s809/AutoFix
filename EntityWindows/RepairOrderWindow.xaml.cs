using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AutoFix
{
    /// <summary>
    /// Логика взаимодействия для WarehouseItemWindow.xaml
    /// </summary>
    public partial class RepairOrderWindow : EntityWindow<RepairOrder>
    {
        public RepairOrderWindow(RepairOrder? repairOrder = null) : base(repairOrder)
        {
            InitializeComponent();
            (masterBox.ItemsSource, masterBox.SelectedIndex) = AppDbContext.GetAllEmployees().WithSelectedIndex(_entity.MasterId);

            var services = AppDbContext.GetAllServices();
            foreach (var entry in _entity.History)
            {
                var fixedService = services.FirstOrDefault(s => s.Id == entry.ServiceId);
                if (fixedService != null)
                    entry.Service = fixedService;
                else
                    services.Add(entry.Service!);
            }
            serviceBox.ItemsSource = services;

            var warehouseItems = AppDbContext.GetAllWarehouseItems();
            foreach (var use in _entity.WarehouseUses)
            {
                var fixedItem = warehouseItems.FirstOrDefault(s => s.Id == use.ItemId);
                if (fixedItem != null)
                    use.Item = fixedItem;
                else
                    warehouseItems.Add(use.Item!);
            }
            warehouseItemBox.ItemsSource = warehouseItems;
        }

        private void AcceptedAmountBox_OnEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                _entity.AcceptedAmount = int.Parse(((NumberBox)sender).Text);
        }
    }
}
