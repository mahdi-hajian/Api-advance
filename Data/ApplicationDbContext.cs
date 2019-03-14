using Common.Utilities;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    public class ApplicationDbContext: DbContext
    {
        // آگر کانکشن استرینگ رو در استارتاپ ساختیم باید این سازنده باشد
        public ApplicationDbContext(DbContextOptions options): base (options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("ConnectionString");
        //    base.OnConfiguring(optionsBuilder); 
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // define assembly interface
            Assembly entitiesAssembly = typeof(IEntity).Assembly;

            // add all entities instead of DbSet<ClassName>
            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);

            // apply all fluent api under Entities
            modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);

            // این متد میاد اون هایی که آن دیلیت روی کسکید هستند رو رستریک میکند 
            // اگر پدری دارای پدری دارای چند فرزند بود اگر پدر را پاک کنیم ارور میدهد و پدر تا بودن فرزندان حذف نمیشوند
            modelBuilder.AddRestrictDeleteBehaviorConvention();

            // هر فیلدی که اسمش آیدی بود رو از نوع جیوآیدی بود رو ریفالت ولیواش رو تغغیر میدهد به NEWSEQUENTIALID() 
            modelBuilder.AddSequentialGuidForIdConvention();

            // اسم جدول هارو جمع میکند مثلا یوزر میشود یوزرز چون رجیستر آل اینتیتیز همان اسم کلاس رو تیبل میکند
            modelBuilder.AddPluralizingTableNameConvention();
        }

        public override int SaveChanges()
        {
            _cleanString();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            _cleanString();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            _cleanString();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _cleanString();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void _cleanString()
        {
            var changedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                    continue;

                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    var propName = property.Name;
                    var val = (string)property.GetValue(item.Entity, null);

                    if (val.HasValue())
                    {
                        var newVal = val.Fa2En().FixPersianChars();
                        if (newVal == val)
                            continue;
                        property.SetValue(item.Entity, newVal, null);
                    }
                }
            }
        }
    }
}
