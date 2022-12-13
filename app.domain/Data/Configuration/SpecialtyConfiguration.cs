using System.Runtime.CompilerServices;
using app.domain.Data.Models;
using app.domain.Data.Utils.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace app.domain.Data.Configuration
{
    public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            builder.ToTable("specialty")
                 .HasKey(s => s.Id);

            builder.HasOne(s => s.Faculty)
                .WithMany(f => f.Specialities)
                .HasForeignKey(fk => fk.FacultyId);
                
            builder.Property(prop => prop.Id)
                .IsRequired()
                .HasColumnType("bigint")
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

            builder.Property(prop => prop.EducationType)
                .HasColumnName("education_type")
                .IsRequired()
                .HasColumnType("character varying")
                .HasConversion(
                    v => v.ToString(),
                    v => (EducationType)Enum.Parse(typeof(EducationType), v))
                .IsUnicode(false);

            builder.Property(prop => prop.ExtrabudgetaryPlaces)
                .HasColumnName("extrabudgetary_places")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(prop => prop.FacultyId)
                .HasColumnName("faculty_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(prop => prop.GeneralCompetition)
                .HasColumnName("general_competition")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(prop => prop.QuotaLOP)
                .HasColumnName("quota_lop")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(prop => prop.SpecialQuota)
                .HasColumnName("special_quota")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(prop => prop.TargetAdmissionQuota)
                .HasColumnName("target_admission_quota")
                .HasColumnType("smallint")
                .IsRequired();

        }
    }
}