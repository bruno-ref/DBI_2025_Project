using Microsoft.EntityFrameworkCore;
using MongoVsRelational.Models;

namespace MongoVsRelational.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dish>()
                .HasMany(d => d.Ingredients)
                .WithMany(i => i.Dishes);
        }
    }
}
