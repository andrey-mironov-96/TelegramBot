using app.domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.domain.Data.Configuration
{
    public class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.ToTable("test", "public")
                .HasKey(s => s.Id);
            BaseConfiguration<Test>.ConfigureBaseModel(builder);

            builder.Property(prop => prop.Title)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("title");
        }
    }
}
