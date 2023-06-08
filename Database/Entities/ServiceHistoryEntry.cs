using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoFix
{
    public class ServiceHistoryEntry : Entity, INotifyPropertyChanged
    {
        private Service? service;
        private bool isCancelled;

        public int OrderId { get; set; }
        public RepairOrder? Order { get; set; }
        
        public int ServiceId { get; set; }
        [Required(ErrorMessage = "Не указана услуга.")]
        public Service? Service
        {
            get => service;
            set
            {
                service = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Service)));
            }
        }

        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly? FinishDate { get; set; }
        public bool IsCancelled
        {
            get => isCancelled;
            set
            {
                isCancelled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCancelled)));
            }
        }
        public string Comments { get; set; } = "";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected override IEnumerable<string> OnValidate()
        {
            if (IsCancelled && FinishDate == null)
                yield return "Для отмены выполнения услуги требуется дата завершения.";
            if (FinishDate != null && FinishDate < StartDate)
                yield return "Дата завершения выполнения услуги не может раньше даты начала.";
        }

        public override void OnSave(AppDbContext ctx)
        {
            ServiceId = Service!.Id;
            ctx.Entry(Service).State = EntityState.Unchanged;
        }
    }
}
