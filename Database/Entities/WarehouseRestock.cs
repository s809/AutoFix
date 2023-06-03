namespace AutoFix
{
    public class WarehouseRestock : Entity
    {
        public int ItemId { get; set; }
        public WarehouseItem? Item { get; set; }

        public int ProviderId { get; set; }
        public WarehouseProvider? Provider { get; set; }

        public int Amount { get; set; }
    }
}
