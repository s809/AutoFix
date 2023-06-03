using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AutoFix
{
    public class Employee : Entity
    {
        private ObservableCollection<EmployeePayout> payouts = new();

        [Required(ErrorMessage = "Не указано ФИО сотрудника.")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Не указаны паспортные данные.")]
        public string PassportInfo { get; set; } = "";

        [Required(ErrorMessage = "Не указана должность.")]
        public string Position { get; set; } = "";

        [Required(ErrorMessage = "Не указан оклад сотрудника.")]
        public decimal BaseSalary { get; set; }

        [Required(ErrorMessage = "Не указана дата наема.")]
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public DateOnly? EndDate { get; set; }
        public string EndReason { get; set; } = "";

        [Required(ErrorMessage = "Не указано имя пользователя.")]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Не указан пароль сотрудника.")]
        public string Password { get; set; } = "";

        public ObservableCollection<RepairOrder> RepairOrders { get; set; } = new();
        public ObservableCollection<EmployeePayout> Payouts { get => payouts; set => payouts = value; }
        public override string? Validate()
        {
            if ((EndDate == null) != (EndReason == ""))
                return "Дата и причина увольнения не могут присутствовать раздельно.";
            if (EndDate != null && EndDate < StartDate)
                return "Дата увольнения не может раньше даты наема.";

            return null;
        }

        public override void OnSave(AppDbContext ctx)
        {
            UpdateCollection(ctx, ctx.EmployeePayouts.Where(ep => ep.EmployeeId == Id).AsEnumerable(), Payouts);
        }

        protected override void OnClone(object cloned)
        {
            CloneCollection(payouts, out ((Employee)cloned).payouts);
        }

        public override string ToString() => Name;
    }
}
