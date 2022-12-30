using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace app.view.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "faculty",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    createat = table.Column<DateTime>(name: "create_at", type: "timestamp without time zone", nullable: false),
                    changeat = table.Column<DateTime>(name: "change_at", type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faculty", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "specialty",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying", nullable: false),
                    generalcompetition = table.Column<short>(name: "general_competition", type: "smallint", nullable: false),
                    quotalop = table.Column<short>(name: "quota_lop", type: "smallint", nullable: false),
                    targetadmissionquota = table.Column<short>(name: "target_admission_quota", type: "smallint", nullable: false),
                    specialquota = table.Column<short>(name: "special_quota", type: "smallint", nullable: false),
                    extrabudgetaryplaces = table.Column<short>(name: "extrabudgetary_places", type: "smallint", nullable: false),
                    educationtype = table.Column<string>(name: "education_type", type: "character varying", unicode: false, nullable: false),
                    facultyid = table.Column<long>(name: "faculty_id", type: "bigint", nullable: false),
                    createat = table.Column<DateTime>(name: "create_at", type: "timestamp without time zone", nullable: false),
                    changeat = table.Column<DateTime>(name: "change_at", type: "timestamp without time zone", nullable: true),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_specialty", x => x.id);
                    table.ForeignKey(
                        name: "FK_specialty_faculty_faculty_id",
                        column: x => x.facultyid,
                        principalTable: "faculty",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_specialty_faculty_id",
                table: "specialty",
                column: "faculty_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "specialty");

            migrationBuilder.DropTable(
                name: "faculty");
        }
    }
}
