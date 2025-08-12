using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineExamSystem.Migrations
{
    public partial class AddSubjectTableAndRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                schema: "exam",
                table: "Exams");

            migrationBuilder.CreateTable(
                name: "Subjects",
                schema: "exam",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectId);
                });

            migrationBuilder.InsertData(
                schema: "exam",
                table: "Subjects",
                columns: new[] { "Name" },
                values: new object[] { "Default Subject" });

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                schema: "exam",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_SubjectId",
                schema: "exam",
                table: "Exams",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Subjects_SubjectId",
                schema: "exam",
                table: "Exams",
                column: "SubjectId",
                principalSchema: "exam",
                principalTable: "Subjects",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Subjects_SubjectId",
                schema: "exam",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_SubjectId",
                schema: "exam",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                schema: "exam",
                table: "Exams");

            migrationBuilder.DeleteData(
                schema: "exam",
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 1);

            migrationBuilder.DropTable(
                name: "Subjects",
                schema: "exam");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                schema: "exam",
                table: "Exams",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
