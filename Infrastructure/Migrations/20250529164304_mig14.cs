using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_AspNetUsers_CreatedByAdminId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_AspNetUsers_CreatedByAdminId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Courses_CourseId1",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_Courses_CourseId1",
                table: "Lectures");

            migrationBuilder.DropIndex(
                name: "IX_Lectures_CourseId1",
                table: "Lectures");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_CourseId1",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_CreatedByAdminId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_CreatedByAdminId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "CourseId1",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Lectures");

            migrationBuilder.DropColumn(
                name: "CourseId1",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "CreatedByAdminId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "SubmissionDate",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "CreatedByAdminId",
                table: "Announcements");

            migrationBuilder.RenameColumn(
                name: "Video",
                table: "Lectures",
                newName: "Topic");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Lectures",
                newName: "InstructorName");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Lectures",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Assignments",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Announcements",
                newName: "Date");

            migrationBuilder.AlterColumn<string>(
                name: "CourseId",
                table: "Lectures",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CourseId",
                table: "Assignments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_CourseId",
                table: "Lectures",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CourseId",
                table: "Assignments",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Courses_CourseId",
                table: "Assignments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_Courses_CourseId",
                table: "Lectures",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Courses_CourseId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_Courses_CourseId",
                table: "Lectures");

            migrationBuilder.DropIndex(
                name: "IX_Lectures_CourseId",
                table: "Lectures");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_CourseId",
                table: "Assignments");

            migrationBuilder.RenameColumn(
                name: "Topic",
                table: "Lectures",
                newName: "Video");

            migrationBuilder.RenameColumn(
                name: "InstructorName",
                table: "Lectures",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Lectures",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Assignments",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Announcements",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Lectures",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CourseId1",
                table: "Lectures",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Lectures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Duration",
                table: "Lectures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "Lectures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Lectures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Lectures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Assignments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CourseId1",
                table: "Assignments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByAdminId",
                table: "Assignments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmissionDate",
                table: "Assignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByAdminId",
                table: "Announcements",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_CourseId1",
                table: "Lectures",
                column: "CourseId1");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CourseId1",
                table: "Assignments",
                column: "CourseId1");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CreatedByAdminId",
                table: "Assignments",
                column: "CreatedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_CreatedByAdminId",
                table: "Announcements",
                column: "CreatedByAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_AspNetUsers_CreatedByAdminId",
                table: "Announcements",
                column: "CreatedByAdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_AspNetUsers_CreatedByAdminId",
                table: "Assignments",
                column: "CreatedByAdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Courses_CourseId1",
                table: "Assignments",
                column: "CourseId1",
                principalTable: "Courses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_Courses_CourseId1",
                table: "Lectures",
                column: "CourseId1",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
