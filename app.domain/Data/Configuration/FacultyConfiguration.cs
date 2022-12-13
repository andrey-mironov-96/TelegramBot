using app.domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.domain.Data.Configuration
{
    public class FacultyConfiguration : IEntityTypeConfiguration<Faculty>
    {
        public void Configure(EntityTypeBuilder<Faculty> builder)
        {
            builder.ToTable("faculty")
                .HasKey(f => f.Id);

            builder.Property(prop => prop.Id)
                .IsRequired()
                .HasColumnName("id");

            builder.Property(prop => prop.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasColumnType("character varying");

            builder.Property(prop => prop.ChangeAt)
                .HasColumnName("change_at")
                .IsRequired(false)
                .HasColumnType("timestamp without time zone");

            builder.Property(prop => prop.CreateAt)
                .HasColumnName("create_at")
                .IsRequired()
                .HasColumnType("timestamp without time zone");

            builder.Property(prop => prop.IsDeleted)
                .HasColumnName("is_deleted")
                .IsRequired()
                .HasColumnType("boolean");
        }
    }
}