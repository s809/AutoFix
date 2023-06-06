using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AutoFix
{
    public class RepairOrder : Entity, INotifyPropertyChanged
    {
        private ObservableCollection<ServiceHistoryEntry> history = new();
        private ObservableCollection<WarehouseUse> warehouseUses = new();
        private decimal acceptedAmount;

        public event PropertyChangedEventHandler? PropertyChanged;
        private void UpdateTotalFieldsByHistory(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var entry in e.OldItems ?? new List<object>())
                ((ServiceHistoryEntry)entry).PropertyChanged -= UpdateTotalFieldsBySingleServiceUse;
            foreach (var entry in e.NewItems ?? new List<object>())
                ((ServiceHistoryEntry)entry).PropertyChanged += UpdateTotalFieldsBySingleServiceUse;

            UpdateTotalFields();
        }
        private void UpdateTotalFieldsBySingleServiceUse(object? sender, PropertyChangedEventArgs e) => UpdateTotalFields();
        private void UpdateTotalFields()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalCost)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChangeAmount)));
        }

        public int MasterId { get; set; }
        public Employee? Master { get; set; }


        [Required(ErrorMessage = "Не указано имя клиента.")]
        public string ClientName { get; set; } = "";
        [Required(ErrorMessage = "Не указан телефон клиента.")]
        public string ClientPhoneNumber { get; set; } = "";


        [Required(ErrorMessage = "Не указан производитель автомобиля.")]
        public string VehicleManufacturer { get; set; } = "";
        [Required(ErrorMessage = "Не указана модель автомобиля.")]
        public string VehicleModel { get; set; } = "";
        [Required(ErrorMessage = "Не указан год выпуска автомобиля.")]
        public int VehicleYear { get; set; } = DateTime.Now.Year;

        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly? FinishDate { get; set; }
        public bool IsCancelled { get; set; }
        public string Comments { get; set; } = "";

        public ObservableCollection<ServiceHistoryEntry> History { get => history; set => history = value; }
        public ObservableCollection<WarehouseUse> WarehouseUses { get => warehouseUses; set => warehouseUses = value; }

        public decimal TotalCost => History.Sum(i => i.Service?.Price ?? 0);
        [Range(0, double.MaxValue, ErrorMessage = "Внесенная сумма должна быть выше 0.")]
        public decimal AcceptedAmount
        {
            get => acceptedAmount;
            set
            {
                acceptedAmount = value;
                UpdateTotalFields();
            }
        }
        public decimal ChangeAmount => AcceptedAmount > TotalCost ? AcceptedAmount - TotalCost : 0;

        protected override void OnClone(object cloned)
        {
            CloneCollection(history, out ((RepairOrder)cloned).history);
            CloneCollection(warehouseUses, out ((RepairOrder)cloned).warehouseUses);
            ((RepairOrder)cloned).history.CollectionChanged += ((RepairOrder)cloned).UpdateTotalFieldsByHistory;
            foreach (var entry in ((RepairOrder)cloned).history)
                entry.PropertyChanged += ((RepairOrder)cloned).UpdateTotalFieldsBySingleServiceUse;
        }

        protected override IEnumerable<string> OnValidate()
        {
            if (IsCancelled && FinishDate == null)
                yield return "Для отмены записи на ремонт требуется дата завершения.";
            if (FinishDate != null && FinishDate < StartDate)
                yield return "Дата завершения записи на ремонт не может раньше даты начала.";

            foreach (var error in History.SelectMany(entry => entry.Validate()))
                yield return error;
            var usesErrors = WarehouseUses.SelectMany(entry => entry.Validate());
            foreach (var error in usesErrors)
                yield return error;

            if (!usesErrors.Any())
            {
                var itemUseAmounts = WarehouseUses.Where(u => !u.Item!.IsDeleted).GroupBy(u => u.Item!.Id).Select(g => new
                {
                    Item = g.First().Item!,
                    Amount = g.Sum(u => u.Amount)
                });

                foreach (var itemUseAmount in itemUseAmounts)
                {
                    var fullUseAmount = itemUseAmount.Amount + itemUseAmount.Item.Uses.Where(u => u.RepairOrderId != Id).Sum(u => u.Amount);
                    var result = itemUseAmount.Item.Restocks.Sum(r => r.Amount) - fullUseAmount;
                    if (result < 0)
                        yield return $"Не хватает {-result} ед. предмета {itemUseAmount.Item}.";
                }
            }
        }

        public override void OnSave(AppDbContext ctx)
        {
            MasterId = Master!.Id;

            UpdateCollection(ctx, ctx.ServiceHistory.Where(sh => sh.OrderId == Id), History);
            UpdateCollection(ctx, ctx.WarehouseUses.Where(wu => wu.RepairOrderId == Id), WarehouseUses);
        }

        public override bool Delete()
        {
            using var ctx = new AppDbContext();
            UpdateCollection(ctx, ctx.ServiceHistory.Where(sh => sh.OrderId == Id), Enumerable.Empty<ServiceHistoryEntry>());
            UpdateCollection(ctx, ctx.WarehouseUses.Where(wu => wu.RepairOrderId == Id), Enumerable.Empty<WarehouseUse>());
            ctx.SaveChanges();
            return base.Delete();
        }

        public override string ToString()
        {
            return $"{ClientName}{(FinishDate != null ? $" (Завершена {FinishDate})" : "")}";
        }
    }
}
