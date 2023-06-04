using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace AutoFix
{
    public class WarehouseProvider : Entity
    {
        [Required(ErrorMessage = "Не указано наименование поставщика.")]
        public string Name { get; set; } = "";
        [Required(ErrorMessage = "Не указана контактная информация.")]
        public string ContactInfo { get; set; } = "";

        public override string ToString()
        {
            return Name;
        }
    }
}
