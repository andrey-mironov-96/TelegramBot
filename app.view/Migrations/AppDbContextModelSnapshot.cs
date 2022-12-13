﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using app.domain.Data.Configuration;

#nullable disable

namespace app.view.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("app.domain.Data.Models.Faculty", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("ChangeAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("change_at");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("create_at");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("faculty", (string)null);
                });

            modelBuilder.Entity("app.domain.Data.Models.Specialty", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("ChangeAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("change_at");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("create_at");

                    b.Property<string>("EducationType")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("character varying")
                        .HasColumnName("education_type");

                    b.Property<short>("ExtrabudgetaryPlaces")
                        .HasColumnType("smallint")
                        .HasColumnName("extrabudgetary_places");

                    b.Property<long>("FacultyId")
                        .HasColumnType("bigint")
                        .HasColumnName("faculty_id");

                    b.Property<short>("GeneralCompetition")
                        .HasColumnType("smallint")
                        .HasColumnName("general_competition");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("character varying")
                        .HasColumnName("name");

                    b.Property<short>("QuotaLOP")
                        .HasColumnType("smallint")
                        .HasColumnName("quota_lop");

                    b.Property<short>("SpecialQuota")
                        .HasColumnType("smallint")
                        .HasColumnName("special_quota");

                    b.Property<short>("TargetAdmissionQuota")
                        .HasColumnType("smallint")
                        .HasColumnName("target_admission_quota");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("specialty", (string)null);
                });

            modelBuilder.Entity("app.domain.Data.Models.Specialty", b =>
                {
                    b.HasOne("app.domain.Data.Models.Faculty", "Faculty")
                        .WithMany("Specialities")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("app.domain.Data.Models.Faculty", b =>
                {
                    b.Navigation("Specialities");
                });
#pragma warning restore 612, 618
        }
    }
}
