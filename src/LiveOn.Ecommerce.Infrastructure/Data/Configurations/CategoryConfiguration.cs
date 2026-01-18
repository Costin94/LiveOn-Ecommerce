using LiveOn.Ecommerce.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace LiveOn.Ecommerce.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            ToTable("Categories");

            HasKey(c => c.Id);

            Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(c => c.CreatedAt).IsRequired();
            Property(c => c.UpdatedAt).IsOptional();
            Property(c => c.IsDeleted).IsRequired();

            Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            Property(c => c.Description)
                .IsOptional()
                .HasMaxLength(500);

            Property(c => c.Slug)
                .IsRequired()
                .HasMaxLength(150);

            Property(c => c.ParentCategoryId)
                .IsOptional();

            Property(c => c.IsActive)
                .IsRequired();

            Property(c => c.DisplayOrder)
                .IsRequired();

            HasOptional(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .WillCascadeOnDelete(false);
        }
    }
}
