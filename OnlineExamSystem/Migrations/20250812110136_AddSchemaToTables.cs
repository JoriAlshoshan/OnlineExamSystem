using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineExamSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddSchemaToTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "exam");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "Questions",
                newSchema: "exam");

            migrationBuilder.RenameTable(
                name: "Options",
                newName: "Options",
                newSchema: "exam");

            migrationBuilder.RenameTable(
                name: "Exams",
                newName: "Exams",
                newSchema: "exam");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Questions",
                schema: "exam",
                newName: "Questions");

            migrationBuilder.RenameTable(
                name: "Options",
                schema: "exam",
                newName: "Options");

            migrationBuilder.RenameTable(
                name: "Exams",
                schema: "exam",
                newName: "Exams");
        }
    }
}
