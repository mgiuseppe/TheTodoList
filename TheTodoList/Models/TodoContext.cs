using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheTodoList.Entities;

namespace TheTodoList.Models
{
    public class TodoContext : IdentityDbContext<StoreUser>
    {
        private readonly string userId;

        public TodoContext(DbContextOptions<TodoContext> options, IUserIdentityProvider userIdentityProvider)
            : base(options)
        {
            this.userId = userIdentityProvider.GetUserId();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Note>().HasQueryFilter(n => n.OwnerId == userId);
        }

        public override int SaveChanges()
        {
            AddOwner();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddOwner();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            AddOwner();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddOwner();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddOwner()
        {
            foreach (var item in ChangeTracker.Entries().Where(e => e.State == EntityState.Added && e.Entity is IHaveAnOwner))
            {
                ((IHaveAnOwner)item.Entity).OwnerId = userId;
            }
        }

        public DbSet<Note> Notes { get; set; }
    }
}
