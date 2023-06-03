using System.Windows;

namespace AutoFix
{
    /// <summary>
    /// Логика взаимодействия для WarehouseItemWindow.xaml
    /// </summary>
    public partial class ServiceWindow : EntityWindow<Service>
    {
        public ServiceWindow(Service? service = null) : base(service) => InitializeComponent();
    }
}
