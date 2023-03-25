using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app.domain.Migrations
{
    /// <inheritdoc />
    public partial class AddPositionToQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "position",
                table: "question",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "position",
                table: "question");
        }
    }
}
