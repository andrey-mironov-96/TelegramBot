using app.domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.domain.Data.Configuration
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("question", "public")
               .HasKey(s => s.Id);

            BaseConfiguration<Question>.ConfigureBaseModel(builder);

            builder.Property(_ => _.TestId)
                .HasColumnName("test_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(prop => prop.Title)
                .HasColumnName("title")
                .HasColumnType("text")
                .IsRequired();

            builder.Property(prop => prop.Position)
                .HasColumnName("position")
                .HasColumnType("smallint")
                .IsRequired();

            builder.HasOne(key => key.Test)
                .WithMany(f => f.Questions)
                .HasForeignKey(fk => fk.TestId);
        }
    }
}
