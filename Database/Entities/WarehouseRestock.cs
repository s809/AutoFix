using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoFix
{
    public class WarehouseRestock : Entity
    {
        public int ItemId { get; set; }
        public WarehouseItem? Item { get; set; }

        public int ProviderId { get; set; }
        [Required(ErrorMessage = "Не указан поставщик заказа.")]
        public WarehouseProvider? Provider { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Количество заказанных предметов должно быть больше 0.")]
        public int Amount { get; set; }

        public override void OnSave(AppDbContext ctx)
        {
            ProviderId = Provider!.Id;
            ctx.Entry(Provider).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
        }
    }
}
