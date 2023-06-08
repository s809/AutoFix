using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFix
{
    public static class EmployeePosition
    {
        public const string
            Administrator = "Администратор",
            WarehouseManager = "Менеджер запчастей",
            ServiceManager = "Менеджер сервиса",
            Mechanic = "Механик",
            Cashier = "Кассир",
            Accountant = "Бухгалтер";

        public static Dictionary<string, string> InternalToDisplayName { get; } = typeof(EmployeePosition).GetFields()
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
            .ToDictionary(f => f.Name, f => (string)f.GetValue(null)!);
        public static List<string> Items { get; } = InternalToDisplayName.Values.ToList();
    }
}
