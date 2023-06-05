using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace AutoFix
{
    public class Service : Entity
    {
        [Required(ErrorMessage = "Необходимо наименование услуги.")]
        public string Name { get; set; } = "";
        [Range(0, int.MaxValue, ErrorMessage = "Стоимоить должна быть не ниже 0.")]
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Price} руб.)";
        }
    }
}
