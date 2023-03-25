using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.domain.Migrations
{
    /// <inheritdoc />
    public partial class AddSchemaPublicInTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "test",
                newName: "test",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "question",
                newName: "question",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "answer",
                newName: "answer",
                newSchema: "public");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "test",
                schema: "public",
                newName: "test");

            migrationBuilder.RenameTable(
                name: "question",
                schema: "public",
                newName: "question");

            migrationBuilder.RenameTable(
                name: "answer",
                schema: "public",
                newName: "answer");
        }
    }
}
