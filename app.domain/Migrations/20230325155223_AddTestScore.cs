using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace app.domain.Migrations
{
    /// <inheritdoc />
    public partial class AddTestScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "test_score",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    test_id = table.Column<long>(type: "bigint", nullable: false),
                    from = table.Column<short>(type: "smallint", nullable: false),
                    to = table.Column<short>(type: "smallint", nullable: false),
                    target_id = table.Column<long>(type: "bigint", nullable: true),
                    target_type = table.Column<string>(type: "text", nullable: false, defaultValue: "None"),
                    text = table.Column<string>(type: "text", nullable: true),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    change_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_score", x => x.id);
                    table.ForeignKey(
                        name: "FK_test_score_test_test_id",
                        column: x => x.test_id,
                        principalSchema: "public",
                        principalTable: "test",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_test_score_test_id",
                schema: "public",
                table: "test_score",
                column: "test_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "test_score",
                schema: "public");
        }
    }
}
