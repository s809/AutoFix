using System;
using System.ComponentModel.DataAnnotations;

namespace AutoFix
{
    public class ServiceHistoryEntry : Entity
    {
        public int OrderId { get; set; }
        [Required]
        public RepairOrder? Order { get; set; }
        
        public int ServiceId { get; set; }
        [Required(ErrorMessage = "Не указана услуга.")]
        public Service? Service { get; set; }

        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly? FinishDate { get; set; }
        public bool IsCancelled { get; set; }
        public string Comments { get; set; } = "";

        public override string? Validate()
        {
            if (FinishDate == null && IsCancelled)
                return "Для отмены выполнения услуги требуется дата завершения.";

            return null;
        }
    }
}
