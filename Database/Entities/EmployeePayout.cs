using System;
using System.ComponentModel.DataAnnotations;

namespace AutoFix
{
    public class EmployeePayout : Entity
    {
        public int EmployeeId { get; set; }
        [Required]
        public Employee? Employee { get; set; }

        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public decimal Amount { get; set; }
        [Required]
        public string Reason { get; set; } = "";

        public override void OnSave(AppDbContext ctx)
        {
            EmployeeId = Employee!.Id;
        }
    }
}
