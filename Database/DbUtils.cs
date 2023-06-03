using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;

namespace AutoFix
{
    public static class DbUtils
    {
        public static bool Save(this Entity entity)
        {
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(entity, new ValidationContext(entity), results, true))
            {
                MessageBox.Show(string.Join("\n", results), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var entityValidationResult = entity.Validate();
            if (entityValidationResult != null)
            {
                MessageBox.Show(entityValidationResult, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            using var ctx = new AppDbContext();
            ctx.Entry(entity).State = entity.Id == 0
                ? EntityState.Added
                : EntityState.Modified;

            entity.OnSave(ctx);

            try
            {
                ctx.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public static (ObservableCollection<T>, int) WithSelectedIndex<T>(this ObservableCollection<T> collection, int id) where T : Entity
            => (collection, Math.Max(0, collection.IndexOf(collection.FirstOrDefault(e => e.Id == id)!)));
    }
}
