using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentMarksheet.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStudentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "StudentMarks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "StudentMarks");
        }
    }
}
