using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig29 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InstructorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    GradeValue = table.Column<int>(type: "int", nullable: false),
                    AssessmentType = table.Column<byte>(type: "tinyint", nullable: false),
                    Term = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateRecorded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicRecords_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcademicRecords_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcademicRecords_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecords_GroupId",
                table: "AcademicRecords",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecords_InstructorId",
                table: "AcademicRecords",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicRecords_StudentId",
                table: "AcademicRecords",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicRecords");
        }
    }
}
