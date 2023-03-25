using app.common.Utils.Enums;
using app.domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.domain.Data.Configuration
{
    public class TestScoreConfiguration : IEntityTypeConfiguration<TestScore>
    {
        public void Configure(EntityTypeBuilder<TestScore> builder)
        {
            builder.ToTable("test_score", "public")
                .HasKey(t => t.Id);

            BaseConfiguration<TestScore>.ConfigureBaseModel(builder);

            builder.Property(_ => _.TestId)
                .HasColumnName("test_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(_ => _.TargetId)
               .HasColumnName("target_id")
               .HasColumnType("bigint")
               .IsRequired(false);

            builder.Property(_ => _.TargetType)
               .HasColumnName("target_type")
               .HasColumnType("text")
               .HasConversion(
                    v => v.ToString(),
                    v => (TargetType)Enum.Parse(typeof(TargetType), v))
               .HasDefaultValue(TargetType.None)
               .IsRequired(true);

            builder.Property(_ => _.Text)
              .HasColumnName("text")
              .HasColumnType("text")
              .IsRequired(false);

            builder.Property(_ => _.From)
                .HasColumnName("from")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(_ => _.To)
                .HasColumnName("to")
                .HasColumnType("smallint")
                .IsRequired();

            builder.HasOne(key => key.Test)
                .WithMany(key => key.TestScores)
                .HasForeignKey(fk => fk.TestId);
        }
    }
}
