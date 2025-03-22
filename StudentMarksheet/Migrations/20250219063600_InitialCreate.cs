using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentMarksheet.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentMarks",
                columns: table => new
                {
                    RollNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalMarks = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentMarks", x => x.RollNumber);
                });

            migrationBuilder.CreateTable(
                name: "Marks",
                columns: table => new
                {
                    RollNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Maths = table.Column<int>(type: "int", nullable: false),
                    Physics = table.Column<int>(type: "int", nullable: false),
                    Chemistry = table.Column<int>(type: "int", nullable: false),
                    SocialStudies = table.Column<int>(type: "int", nullable: false),
                    SecondLanguage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marks", x => x.RollNumber);
                    table.ForeignKey(
                        name: "FK_Marks_StudentMarks_RollNumber",
                        column: x => x.RollNumber,
                        principalTable: "StudentMarks",
                        principalColumn: "RollNumber",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Marks");

            migrationBuilder.DropTable(
                name: "StudentMarks");
        }
    }
}
