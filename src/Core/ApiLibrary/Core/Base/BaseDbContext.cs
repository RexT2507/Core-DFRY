using ApiLibrary.Core.Attributes;
using Auth.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApiLibrary.Core.Base
{
    public class BaseDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public BaseDbContext() : base()
        {

        }

        public BaseDbContext(DbContextOptions options) : base(options)
        {

        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddTracking();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            AddTracking();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddTracking()
        {

            var entries = ChangeTracker.Entries().Where(s => s.Entity is BaseEntity && (s.State == EntityState.Modified || s.State == EntityState.Added || s.State == EntityState.Deleted));
            foreach (var item in entries)
            {


                if (item.State == EntityState.Modified)
                {
                    // --- La ligne commentée c'est si on veut récupérer que les attributs de BaseEntity et pas celles de tout le modèle --- //

                    //var test = typeof(BaseEntity).GetProperties();

                    var properties = item.Entity.GetType().GetProperties();
                    foreach (var prop in properties)
                    {
                        //Mon lastState aussi il fonctionne! :P
                        if (prop.GetCustomAttribute<OmittedAttribute>() != null /*&& lastState != EntityState.Deleted*/)
                        {
                            item.Property(prop.Name).IsModified = false;
                        }
                    }
                }


                if (item.State == EntityState.Deleted)
                {
                    //lastState = item.State;
                    ((BaseEntity)item.Entity).deletedAt = DateTime.Now;
                    item.State = EntityState.Modified;
                }

                if (item.State == EntityState.Added)
                {
                    ((BaseEntity)item.Entity).createdAt = DateTime.Now;
                }
                else
                {
                    ((BaseEntity)item.Entity).updatedAt = DateTime.Now;
                }
            }


        }
    }
}
