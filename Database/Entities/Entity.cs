using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;

namespace AutoFix
{
    public abstract class Entity : ICloneable
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        #region Validate
        public IEnumerable<string> Validate()
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this), results, true);

            return results.Select(x => x.ToString()).Concat(OnValidate());
        }
        protected virtual IEnumerable<string> OnValidate() => Enumerable.Empty<string>();
        #endregion

        #region Save
        public bool Save()
        {
            var validationResults = Validate().ToList();
            if (validationResults.Count > 0)
            {
                MessageBox.Show(string.Join("\n", validationResults), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            using var ctx = new AppDbContext();
            OnSave(ctx);
            Upsert(ctx);

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
        public virtual void OnSave(AppDbContext ctx) { }
        protected static void UpdateCollection<T>(AppDbContext ctx, IEnumerable<T> inDb, IEnumerable<T> updated, bool softDelete = false) where T : Entity
        {
            var toDelete = inDb.Except(updated, EqualityComparer);
            
            if (softDelete)
            {
                foreach (var item in toDelete)
                    item.IsDeleted = true;
            }
            else
            {
                ctx.RemoveRange(toDelete);
            }
            
            foreach (var entity in updated)
            {
                entity.OnSave(ctx);
                entity.Upsert(ctx);
            }
        }
        public void Upsert(AppDbContext ctx)
        {
            ctx.Entry(this).State = Id == 0
                ? EntityState.Added
                : EntityState.Modified;
        }
        #endregion

        #region Clone
        public object Clone()
        {
            var cloned = MemberwiseClone();
            OnClone(cloned);
            return cloned;
        }
        protected virtual void OnClone(object cloned) { }
        protected static void CloneCollection<T>(ObservableCollection<T> original, out ObservableCollection<T> cloned) where T : Entity
        {
            cloned = new ObservableCollection<T>(original.Select(e => (T)e.Clone()));
        }
        public static T Clone<T>(object entity) where T : Entity => (T)((T)entity).Clone();
        #endregion

        #region Delete
        public virtual bool Delete()
        {
            IsDeleted = true;
            Save();
            return true;
        }
        #endregion

        public static EntityEqualityComparer EqualityComparer { get; } = new();
        public class EntityEqualityComparer : IEqualityComparer<Entity>
        {
            public bool Equals(Entity? x, Entity? y) => x?.Id != 0 && x?.Id != 0 && x?.Id == y?.Id;
            public int GetHashCode(Entity obj) => obj.Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} <{Id}>";
        }
    }
}
