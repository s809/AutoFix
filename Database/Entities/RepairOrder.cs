using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AutoFix
{
    public class RepairOrder : Entity
    {
        private ObservableCollection<ServiceHistoryEntry> history = new();
        private ObservableCollection<WarehouseUse> warehouseUses = new();

        public int MasterId { get; set; }
        [Required]
        public Employee? Master { get; set; }

        [Required(ErrorMessage = "Не указано имя клиента.")]
        public string ClientName { get; set; } = "";
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly? FinishDate { get; set; }
        public bool IsCancelled { get; set; }
        public string Comments { get; set; } = "";

        public ObservableCollection<ServiceHistoryEntry> History { get => history; set => history = value; }
        public ObservableCollection<WarehouseUse> WarehouseUses { get => warehouseUses; set => warehouseUses = value; }

        protected override void OnClone(object cloned)
        {
            CloneCollection(history, out ((RepairOrder)cloned).history);
            CloneCollection(warehouseUses, out ((RepairOrder)cloned).warehouseUses);
        }
        public override string? Validate()
        {
            if (FinishDate == null && IsCancelled)
                return "Для отмены записи на ремонт требуется дата завершения.";
            if (FinishDate != null && FinishDate < StartDate)
                return "Дата завершения записи на ремонт не может раньше даты наема.";

            return null;
        }

        public override void OnSave(AppDbContext ctx)
        {
            MasterId = Master!.Id;
            UpdateCollection(ctx, ctx.ServiceHistory.Where(sh => sh.Id == Id), History);
            UpdateCollection(ctx, ctx.WarehouseUses.Where(wu => wu.Id == Id), WarehouseUses);
        }
    }
}
