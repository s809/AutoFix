using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace AutoFix
{
    public class Service : Entity
    {
        private ObservableCollection<ServiceHistoryEntry> historyEntries = new();

        [Required(ErrorMessage = "Необходимо наименование услуги.")]
        public string Name { get; set; } = "";
        [Range(0, int.MaxValue, ErrorMessage = "Стоимоить должна быть не ниже 0.")]
        public decimal Price { get; set; }

        public ObservableCollection<ServiceHistoryEntry> HistoryEntries { get => historyEntries; set => historyEntries = value; }
        protected override void OnClone(object cloned)
        {
            CloneCollection(historyEntries, out ((Service)cloned).historyEntries);
        }
    }
}
