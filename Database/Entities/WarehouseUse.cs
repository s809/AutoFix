using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AutoFix
{
    public class WarehouseUse : Entity
    {
        public int RepairOrderId { get; set; }
        public RepairOrder? RepairOrder { get; set; }

        public int ItemId { get; set; }
        [Required(ErrorMessage = "Не указана предмет.")]
        public WarehouseItem? Item { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Должна быть использована минимум 1 единица предмета.")]
        public int Amount { get; set; }

        public override void OnSave(AppDbContext ctx)
        {
            ItemId = Item!.Id;
            ctx.Entry(Item).State = EntityState.Unchanged;
        }
    }
}
