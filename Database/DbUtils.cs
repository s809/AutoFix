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
            Validator.TryValidateObject(entity, new ValidationContext(entity), results, true);

            var stringResults = results.Select(x => x.ToString()).Concat(entity.Validate()).ToList();
            if (stringResults.Count != 0)
            {
                MessageBox.Show(string.Join("\n", stringResults), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            using var ctx = new AppDbContext();
            entity.OnSave(ctx);
            entity.Upsert(ctx);

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
