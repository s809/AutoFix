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
            {
                var fixedProvider = providers.FirstOrDefault(s => s.Id == entry.ProviderId);
                if (fixedProvider != null)
                    entry.Provider = fixedProvider;
                else
                    providers.Add(entry.Provider!);
            }
            providerBox.ItemsSource = providers;
        }
    }
}
