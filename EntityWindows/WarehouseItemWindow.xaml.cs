using System.Linq;
using System.Windows;

namespace AutoFix
{
    /// <summary>
    /// Логика взаимодействия для WarehouseItemWindow.xaml
    /// </summary>
    public partial class WarehouseItemWindow : EntityWindow<WarehouseItem>
    {
        public WarehouseItemWindow(WarehouseItem? warehouseItem = null) : base(warehouseItem)
        {
            InitializeComponent();

            var providers = AppDbContext.GetAllWarehouseProviders();
            foreach (var entry in _entity.Restocks)
                entry.Provider = providers.First(s => s.Id == entry.ProviderId);
            providerBox.ItemsSource = providers;
        }
    }
}
