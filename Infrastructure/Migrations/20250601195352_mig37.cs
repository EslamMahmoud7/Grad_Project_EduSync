using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig37 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicRecords_Instructors_InstructorId",
                table: "AcademicRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Instructors_InstructorId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropIndex(
                name: "IX_AcademicRecords_InstructorId",
                table: "AcademicRecords");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "AcademicRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_AspNetUsers_InstructorId",
                table: "Groups",
                column: "InstructorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_InstructorId",
                table: "Groups");

            migrationBuilder.AddColumn<string>(
                name: "InstructorId",
                table: "AcademicRecords",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecords_InstructorId",
                table: "AcademicRecords",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicRecords_Instructors_InstructorId",
                table: "AcademicRecords",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Instructors_InstructorId",
                table: "Groups",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
