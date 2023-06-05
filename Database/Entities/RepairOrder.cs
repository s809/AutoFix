﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        protected override void OnClone(object cloned)
        {
            CloneCollection(history, out ((RepairOrder)cloned).history);
            CloneCollection(warehouseUses, out ((RepairOrder)cloned).warehouseUses);
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
                var itemUseAmounts = WarehouseUses.GroupBy(u => u.Item!.Id).Select(g => new
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
    }
}
