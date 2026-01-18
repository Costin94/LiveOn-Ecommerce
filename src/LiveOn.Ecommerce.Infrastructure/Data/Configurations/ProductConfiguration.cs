using LiveOn.Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveOn.Ecommerce.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            ToTable("Products");

            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.CreatedAt).IsRequired();
            Property(p => p.UpdatedAt).IsOptional();
            Property(p => p.IsDeleted).IsRequired();

            Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            Property(p => p.SKU)
                .IsRequired()
                .HasMaxLength(50);

            Property(p => p.SKU)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("IX_Product_SKU") { IsUnique = true }));

            Property(p => p.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            Property(p => p.StockQuantity)
                .IsRequired();

            Property(p => p.Status)
                .IsRequired();

            Property(p => p.ImageUrl)
                .IsOptional()
                .HasMaxLength(500);

            Property(p => p.Weight)
                .IsOptional()
                .HasPrecision(10, 2);

            Property(p => p.IsFeatured)
                .IsOptional();

            Property(p => p.DiscountPercentage)
                .IsOptional()
                .HasPrecision(5, 2);

            Property(p => p.CategoryId)
                .IsRequired();

            HasRequired(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .WillCascadeOnDelete(false);

            Ignore(p => p.FinalPrice);
            Ignore(p => p.IsInStock);
            Ignore(p => p.IsAvailable);
        }
    }
}
