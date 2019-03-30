using Common.Utilities;
using EFSecondLevelCache.Core;
using EFSecondLevelCache.Core.Contracts;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        // آگر کانکشن استرینگ رو در استارتاپ ساختیم باید این سازنده باشد
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("ConnectionString");
        //    base.OnConfiguring(optionsBuilder); 
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // باید اولین متد باشد
            base.OnModelCreating(modelBuilder);

            // define assembly interface
            Assembly entitiesAssembly = typeof(IEntity).Assembly;

            // add all entities instead of DbSet<ClassName>
            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);

            // apply all fluent api under Entities
            modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);

            // این متد میاد اون هایی که آن دیلیت روی کسکید هستند رو رستریک میکند 
            // اگر پدری دارای پدری دارای چند فرزند بود اگر پدر را پاک کنیم ارور میدهد و پدر تا بودن فرزندان حذف نمیشوند
            //modelBuilder.AddRestrictDeleteBehaviorConvention();

            // هر فیلدی که اسمش آیدی بود رو از نوع جیوآیدی بود رو ریفالت ولیواش رو تغغیر میدهد به NEWSEQUENTIALID() 
            modelBuilder.AddSequentialGuidForIdConvention();

            // اسم جدول هارو جمع میکند مثلا یوزر میشود یوزرز چون رجیستر آل اینتیتیز همان اسم کلاس رو تیبل میکند
            modelBuilder.AddPluralizingTableNameConvention();

            #region seed
            modelBuilder.Entity<Role>().HasData(
                new Role
                    {
                        Id = 1,
                        Name = "Leader",
                        Description = "ادمین اصلی سایت",
                        NormalizedName = "LEADER"
                    },
                    new Role
                    {
                        Id = 2,
                        Name = "Admin",
                        Description = "جانشین",
                        NormalizedName = "ADMIN"
                    }
                );
            #endregion

            #region Change Name
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaim");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogin");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserToken");
            #endregion
        }

        #region replacestrignPersianBug
        public override int SaveChanges()
        {
            this.ChangeTracker.DetectChanges();
            var changedEntityNames = this.GetChangedEntityNames();

            _cleanString();
            var result = base.SaveChanges();
            this.GetService<IEFCacheServiceProvider>().InvalidateCacheDependencies(changedEntityNames);

            return result;

            #region Old
            //_cleanString();
            //return base.SaveChanges();
            #endregion
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ChangeTracker.DetectChanges();
            var changedEntityNames = this.GetChangedEntityNames();

            _cleanString();
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            this.GetService<IEFCacheServiceProvider>().InvalidateCacheDependencies(changedEntityNames);

            return result;

            #region Old
            //_cleanString();
            //return base.SaveChanges(acceptAllChangesOnSuccess);
            #endregion
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            this.ChangeTracker.DetectChanges();
            var changedEntityNames = this.GetChangedEntityNames();

            _cleanString();
            var result = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            this.GetService<IEFCacheServiceProvider>().InvalidateCacheDependencies(changedEntityNames);

            return result;

            #region Old
            //_cleanString();
            //return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            #endregion
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.ChangeTracker.DetectChanges();
            var changedEntityNames = this.GetChangedEntityNames();

            _cleanString();
            var result = base.SaveChangesAsync(cancellationToken);
            this.GetService<IEFCacheServiceProvider>().InvalidateCacheDependencies(changedEntityNames);

            return result;

            #region Old
            //_cleanString();
            //return base.SaveChangesAsync(cancellationToken);
            #endregion
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
                    string propName = property.Name;
                    string val = (string)property.GetValue(item.Entity, null);

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

        #endregion
    }
}
