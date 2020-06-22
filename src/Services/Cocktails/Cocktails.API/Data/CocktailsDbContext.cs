using ApiLibrary.Core.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cocktails.API.Models;

namespace Cocktails.API.Data
{
    public class CocktailsDbContext : BaseDbContext
    {
        public DbSet<Cocktail> Cocktails { get; set; }

        public CocktailsDbContext(DbContextOptions options) : base(options)
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

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<Preparation> Preparations { get; set; }
    }
}
