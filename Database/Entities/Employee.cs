using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AutoFix
{
    public class Employee : Entity
    {
        private ObservableCollection<EmployeePayout> payouts = new();

        [Required(ErrorMessage = "Не указана фамилия сотрудника.")]
        public string Surname { get; set; } = "";
        [Required(ErrorMessage = "Не указано имя сотрудника.")]
        public string Name { get; set; } = "";
        [Required(ErrorMessage = "Не указано отчество сотрудника.")]
        public string Patronymic { get; set; } = "";

        public string FullName => $"{Surname} {Name} {Patronymic}";

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
        protected override IEnumerable<string> OnValidate()
        {
            if ((EndDate == null) != (EndReason == ""))
                yield return "Дата и причина увольнения не могут присутствовать раздельно.";
            if (EndDate != null && EndDate < StartDate)
                yield return "Дата увольнения не может раньше даты наема.";
        }

        public override void OnSave(AppDbContext ctx)
        {
            UpdateCollection(ctx, ctx.EmployeePayouts.Where(ep => ep.EmployeeId == Id), Payouts);
        }

        protected override void OnClone(object cloned)
        {
            CloneCollection(payouts, out ((Employee)cloned).payouts);
        }

        public override string ToString() => FullName;
    }
}
