using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace onlineExamApp.Migrations
{
    /// <inheritdoc />
    public partial class AddEducatorIdToStudentExamAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EducatorId",
                table: "StudentExamAttempts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentExamAttempts_EducatorId",
                table: "StudentExamAttempts",
                column: "EducatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentExamAttempts_AspNetUsers_EducatorId",
                table: "StudentExamAttempts",
                column: "EducatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentExamAttempts_AspNetUsers_EducatorId",
                table: "StudentExamAttempts");

            migrationBuilder.DropIndex(
                name: "IX_StudentExamAttempts_EducatorId",
                table: "StudentExamAttempts");

            migrationBuilder.DropColumn(
                name: "EducatorId",
                table: "StudentExamAttempts");
        }
    }
}
