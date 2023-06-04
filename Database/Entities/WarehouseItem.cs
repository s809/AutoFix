using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AutoFix
{
    public class WarehouseItem : Entity
    {
        private ObservableCollection<WarehouseRestock> restocks = new();

        [Required(ErrorMessage = "Не указано наименование предмета.")]
        public string Name { get; set; } = "";
        [Required(ErrorMessage = "Не указан производитель предмета.")]
        public string Manufacturer { get; set; } = "";
        [Range(0, int.MaxValue, ErrorMessage = "Стоимость не может быть меньше 0.")]
        public decimal Price { get; set; }

        public ObservableCollection<WarehouseRestock> Restocks { get => restocks; set => restocks = value; }
        protected override void OnClone(object cloned)
        {
            CloneCollection(restocks, out ((WarehouseItem)cloned).restocks);
        }

        protected override IEnumerable<string> OnValidate()
        {
            foreach (var error in restocks.SelectMany(r => r.Validate()))
                yield return error;
        }

        public override void OnSave(AppDbContext ctx)
        {
            UpdateCollection(ctx, ctx.WarehouseRestocks.Where(r => r.ItemId == Id), restocks);
        }

        public override string ToString()
        {
            return $"{Name} ({Manufacturer})";
        }
    }
}
