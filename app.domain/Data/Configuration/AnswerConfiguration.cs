using app.domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.domain.Data.Configuration
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("answer", "public");
            BaseConfiguration<Answer>.ConfigureBaseModel(builder);
            builder.Property(prop => prop.Text)
                .HasColumnName("text")
                .HasColumnType("text")
                .IsRequired();

            builder.Property(prop => prop.QuestionId)
                .HasColumnName("question_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(prop => prop.Point)
                .HasColumnName("point")
                .HasColumnType("smallint");

            builder.HasOne(k => k.Question)
                .WithMany(f => f.Answers)
                .HasForeignKey(fk => fk.QuestionId);
        }
    }
}
