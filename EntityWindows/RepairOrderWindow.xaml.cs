using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

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
                entry.Service = services.First(s => s.Id == entry.ServiceId);
            serviceBox.ItemsSource = services;
        }
    }
}
