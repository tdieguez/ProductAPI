using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyStore.OpenApi.Entities;

namespace MyStore.OpenApi.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(150);
        }
    }
}