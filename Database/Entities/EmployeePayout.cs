using System;
using System.ComponentModel.DataAnnotations;

namespace AutoFix
{
    public class EmployeePayout : Entity
    {
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        [Range(1, double.MaxValue, ErrorMessage = "Сумма выплаты должна быть больше 0.")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Не указана причина выплаты.")]
        public string Reason { get; set; } = "Зарплата";
    }
}
