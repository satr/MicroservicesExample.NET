using System.Data.Entity;
using MicroservicesExample.NET.Products.Service;

namespace Products.Service.DataLayer
{
    internal class ProductsDbContext : DbContext, IProductsDbContext
    {
        public ProductsDbContext(string connectionString) 
            : base(connectionString)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ProductsDbContext>());
        }

        public virtual IDbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var entity = modelBuilder.Entity<Product>()
                                    .ToTable("Product")
                                    .HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(40);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Price).IsRequired().HasColumnType("FLOAT");
        }
    }
}