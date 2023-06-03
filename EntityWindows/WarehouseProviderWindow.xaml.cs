using System.Windows;

namespace AutoFix
{
    /// <summary>
    /// Логика взаимодействия для WarehouseItemWindow.xaml
    /// </summary>
    public partial class WarehouseProviderWindow : EntityWindow<WarehouseProvider>
    {
        public WarehouseProviderWindow(WarehouseProvider? warehouseItem = null) : base(warehouseItem) => InitializeComponent();
    }
}
