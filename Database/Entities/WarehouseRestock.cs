using System.Collections.Generic;

namespace AutoFix
{
    public class WarehouseRestock : Entity
    {
        public int ItemId { get; set; }
        public WarehouseItem? Item { get; set; }

        public int ProviderId { get; set; }
        public WarehouseProvider? Provider { get; set; }

        public int Amount { get; set; }

        public override void OnSave(AppDbContext ctx)
        {
            ProviderId = Provider!.Id;
            ctx.Entry(Provider).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
        }
    }
}
