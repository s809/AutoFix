using System.ComponentModel.DataAnnotations;

namespace AutoFix
{
    public class WarehouseUse : Entity
    {
        public int RepairOrderId { get; set; }
        public RepairOrder? RepairOrder { get; set; }

        public int ItemId { get; set; }
        public WarehouseItem? Item { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Должен быть использован минимум 1 предмет.")]
        public int Amount { get; set; }
    }
}
