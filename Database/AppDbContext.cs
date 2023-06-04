using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;

namespace AutoFix
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeePayout> EmployeePayouts { get; set; }
        public DbSet<RepairOrder> RepairOrders { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceHistoryEntry> ServiceHistory { get; set; }
        public DbSet<WarehouseItem> WarehouseItems { get; set; }
        public DbSet<WarehouseProvider> WarehouseProviders { get; set; }
        public DbSet<WarehouseUse> WarehouseUses { get; set; }
        public DbSet<WarehouseRestock> WarehouseRestocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .EnableSensitiveDataLogging()
            .UseNpgsql("Host=localhost;Database=AutoFix;Username=postgres;Password=root;Include Error Detail=true");
            //.UseNpgsql("Host=localhost;Database=AutoFix;Username=postgres;Password=22345621;Include Error Detail=true");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasMany<RepairOrder>().WithOne(o => o.Master);
            modelBuilder.Entity<Employee>().HasMany(e => e.Payouts).WithOne(p => p.Employee);
            modelBuilder.Entity<Service>().HasMany<ServiceHistoryEntry>().WithOne(e => e.Service);
            modelBuilder.Entity<RepairOrder>().HasMany(o => o.History).WithOne(e => e.Order);
            modelBuilder.Entity<RepairOrder>().HasMany(e => e.WarehouseUses).WithOne(u => u.RepairOrder);
            modelBuilder.Entity<WarehouseItem>().HasMany<WarehouseUse>().WithOne(u => u.Item);
            modelBuilder.Entity<WarehouseItem>().HasMany(i => i.Restocks).WithOne(r => r.Item);
            modelBuilder.Entity<WarehouseProvider>().HasMany<WarehouseRestock>().WithOne(r => r.Provider);
        }

        public static Employee? FindLoginEmployee(string username, string password)
        {
            using var ctx = new AppDbContext();
            return ctx.Employees.FirstOrDefault(e => e.Username == username && e.Password == password);
        }

        public static int CountEmployees()
        {
            using var ctx = new AppDbContext();
            return ctx.Employees.Count();
        }

        public static ObservableCollection<Employee> GetAllEmployees()
        {
            using var ctx = new AppDbContext();
            return new ObservableCollection<Employee>(
                ctx.Employees
                .Include(e => e.Payouts)
            );
        }

        public static ObservableCollection<WarehouseItem> GetAllWarehouseItems()
        {
            using var ctx = new AppDbContext();
            return new ObservableCollection<WarehouseItem>(
                ctx.WarehouseItems
                .Include(i => i.Restocks)
            );
        }

        public static ObservableCollection<WarehouseProvider> GetAllWarehouseProviders()
        {
            using var ctx = new AppDbContext();
            return new ObservableCollection<WarehouseProvider>(ctx.WarehouseProviders);
        }

        public static ObservableCollection<RepairOrder> GetAllRepairOrders()
        {
            using var ctx = new AppDbContext();
            return new ObservableCollection<RepairOrder>(
                ctx.RepairOrders
                .Include(ro => ro.History)
                .Include(ro => ro.WarehouseUses)
            );
        }

        public static ObservableCollection<Service> GetAllServices()
        {
            using var ctx = new AppDbContext();
            return new ObservableCollection<Service>(ctx.Services);
        }
    }
}
