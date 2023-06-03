using System.ComponentModel.DataAnnotations;

namespace AutoFix
{
    public class WarehouseItem : Entity
    {
        [Required(ErrorMessage = "Не указано наименование предмета.")]
        public string Name { get; set; } = "";
        [Required(ErrorMessage = "Не указан производитель предмета.")]
        public string Manufacturer { get; set; } = "";
        [Range(0, int.MaxValue, ErrorMessage = "Стоимость не может быть меньше 0.")]
        public decimal Price { get; set; }
    }
}
