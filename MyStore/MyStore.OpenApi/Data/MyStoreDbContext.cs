using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MyStore.OpenApi.Entities;

namespace MyStore.OpenApi.Data
{
    public class MyStoreDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }


        public MyStoreDbContext(DbContextOptions<MyStoreDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}