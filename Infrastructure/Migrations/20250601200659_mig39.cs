using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig39 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstructorId",
                table: "AcademicRecords",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecords_InstructorId",
                table: "AcademicRecords",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicRecords_AspNetUsers_InstructorId",
                table: "AcademicRecords",
                column: "InstructorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicRecords_AspNetUsers_InstructorId",
                table: "AcademicRecords");

            migrationBuilder.DropIndex(
                name: "IX_AcademicRecords_InstructorId",
                table: "AcademicRecords");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "AcademicRecords");
        }
    }
}
