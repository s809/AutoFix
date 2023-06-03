using System.Windows;

namespace AutoFix
{
    /// <summary>
    /// Логика взаимодействия для WarehouseItemWindow.xaml
    /// </summary>
    public partial class RepairOrderWindow : EntityWindow<RepairOrder>
    {
        public RepairOrderWindow(RepairOrder? warehouseItem = null) : base(warehouseItem)
        {
            InitializeComponent();
            (masterBox.ItemsSource, masterBox.SelectedIndex) = AppDbContext.GetAllEmployees().WithSelectedIndex(_entity.MasterId);
        }
    }
}
