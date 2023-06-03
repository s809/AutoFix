using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace AutoFix
{
    public class WarehouseProvider : Entity
    {
        private ObservableCollection<WarehouseRestock> restocks = new();

        [Required(ErrorMessage = "Не указано наименование поставщика.")]
        public string Name { get; set; } = "";
        [Required(ErrorMessage = "Не указана контактная информация.")]
        public string ContactInfo { get; set; } = "";

        public ObservableCollection<WarehouseRestock> Restocks { get => restocks; set => restocks = value; }
        protected override void OnClone(object cloned)
        {
            CloneCollection(restocks, out ((WarehouseProvider)cloned).restocks);
        }
    }
}
