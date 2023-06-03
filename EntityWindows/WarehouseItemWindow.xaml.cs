using System.Windows;

namespace AutoFix
{
    /// <summary>
    /// Логика взаимодействия для WarehouseItemWindow.xaml
    /// </summary>
    public partial class WarehouseItemWindow : EntityWindow<WarehouseItem>
    {
        public WarehouseItemWindow(WarehouseItem? warehouseItem = null) : base(warehouseItem) => InitializeComponent();
    }
}
