using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutoFix
{
    public abstract class Entity : ICloneable
    {
        public int Id { get; set; }

        public virtual IEnumerable<string> Validate() => Enumerable.Empty<string>();
        public virtual void OnSave(AppDbContext ctx) { }

        public object Clone()
        {
            var cloned = MemberwiseClone();
            OnClone(cloned);
            return cloned;
        }
        protected virtual void OnClone(object cloned) { }
        protected static void CloneCollection<T>(ObservableCollection<T> original, out ObservableCollection<T> cloned) where T : Entity
        {
            cloned = new ObservableCollection<T>(original);
        }

        public static T Clone<T>(object entity) where T : Entity => (T)((T)entity).Clone();

        protected void UpdateCollection<T>(AppDbContext ctx, IEnumerable<T> inDb, IEnumerable<T> updated) where T : Entity
        {
            ctx.RemoveRange(inDb.Except(updated, EqualityComparer));
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
