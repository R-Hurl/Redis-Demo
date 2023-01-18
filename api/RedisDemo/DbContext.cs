using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedisDemo.Models;

namespace RedisDemo
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            new CategoriesMap(builder.Entity<Category>());
            new ProductsMap(builder.Entity<Product>());
        }
    }

    internal class CategoriesMap
    {
        public CategoriesMap(EntityTypeBuilder<Category> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.ToTable("categories");

            // Columns
            entityBuilder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entityBuilder.Property(x => x.CategoryName).HasColumnName("category_name");
        }
    }

    internal class ProductsMap
    {
        public ProductsMap(EntityTypeBuilder<Product> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.ToTable("products");

            // Columns
            entityBuilder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entityBuilder.Property(x => x.CategoryId).HasColumnName("category_id");
            entityBuilder.Property(x => x.Name).HasColumnName("name");
            entityBuilder.Property(x => x.Description).HasColumnName("description");
            entityBuilder.Property(x => x.Price).HasColumnName("price");
        }
    }
}