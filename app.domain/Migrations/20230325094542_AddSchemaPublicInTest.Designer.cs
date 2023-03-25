﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using app.domain.Data.Configuration;

#nullable disable

namespace app.domain.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230325094542_AddSchemaPublicInTest")]
    partial class AddSchemaPublicInTest
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("app.domain.Data.Models.Answer", b =>
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

                    b.Property<short>("Point")
                        .HasColumnType("smallint")
                        .HasColumnName("point");

                    b.Property<long>("QuestionId")
                        .HasColumnType("bigint")
                        .HasColumnName("question_id");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("answer", "public");
                });

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

            modelBuilder.Entity("app.domain.Data.Models.Question", b =>
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

                    b.Property<short>("Position")
                        .HasColumnType("smallint")
                        .HasColumnName("position");

                    b.Property<long>("TestId")
                        .HasColumnType("bigint")
                        .HasColumnName("test_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("question", "public");
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

            modelBuilder.Entity("app.domain.Data.Models.Test", b =>
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

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("test", "public");
                });

            modelBuilder.Entity("app.domain.Data.Models.Answer", b =>
                {
                    b.HasOne("app.domain.Data.Models.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("app.domain.Data.Models.Question", b =>
                {
                    b.HasOne("app.domain.Data.Models.Test", "Test")
                        .WithMany("Questions")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Test");
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

            modelBuilder.Entity("app.domain.Data.Models.Question", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("app.domain.Data.Models.Test", b =>
                {
                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}
