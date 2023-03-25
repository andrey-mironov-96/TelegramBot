using app.domain.Data.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.domain.Data.Configuration
{
    public class BaseConfiguration<T>
        where T : ABaseModel
    {
        public static void ConfigureBaseModel(EntityTypeBuilder<T> builder)
        {
            builder.Property(prop => prop.Id)
                .IsRequired()
                .HasColumnType("bigint")
                .HasColumnName("id");

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
