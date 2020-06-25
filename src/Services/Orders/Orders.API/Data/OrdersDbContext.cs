using ApiLibrary.Core.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.API.Models;

namespace Orders.API.Data
{
    public class OrdersDbContext : BaseDbContext
    {
        public DbSet<Order> Orders { get; set; }

        public OrdersDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
