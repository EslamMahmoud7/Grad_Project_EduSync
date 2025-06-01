using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Courses_CourseId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Courses_CourseId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Instructors_InstructorId1",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_AspNetUsers_StudentId",
                table: "GroupStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Courses_CourseId1",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Courses_CourseId1",
                table: "Quizzes");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "StudentCourses");

            migrationBuilder.DropIndex(
                name: "IX_Groups_InstructorId1",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "InstructorId1",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "InstructorEmail",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "InstructorName",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "NextDeadline",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "CourseId1",
                table: "Quizzes",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_CourseId1",
                table: "Quizzes",
                newName: "IX_Quizzes_GroupId");

            migrationBuilder.RenameColumn(
                name: "CourseId1",
                table: "Materials",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Materials_CourseId1",
                table: "Materials",
                newName: "IX_Materials_GroupId");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Assignments",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_CourseId",
                table: "Assignments",
                newName: "IX_Assignments_GroupId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Institution",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AvatarUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Achievements",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "RecentActivity",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "SocialLinks",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Groups_GroupId",
                table: "Assignments",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Courses_CourseId",
                table: "Groups",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_AspNetUsers_StudentId",
                table: "GroupStudents",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Groups_GroupId",
                table: "Materials",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Groups_GroupId",
                table: "Quizzes",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Groups_GroupId",
                table: "Assignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Courses_CourseId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_AspNetUsers_StudentId",
                table: "GroupStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Groups_GroupId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Groups_GroupId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Achievements",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RecentActivity",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SocialLinks",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Quizzes",
                newName: "CourseId1");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_GroupId",
                table: "Quizzes",
                newName: "IX_Quizzes_CourseId1");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Materials",
                newName: "CourseId1");

            migrationBuilder.RenameIndex(
                name: "IX_Materials_GroupId",
                table: "Materials",
                newName: "IX_Materials_CourseId1");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Assignments",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Assignments_GroupId",
                table: "Assignments",
                newName: "IX_Assignments_CourseId");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Quizzes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InstructorId1",
                table: "Groups",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstructorEmail",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstructorName",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "NextDeadline",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Institution",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AvatarUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Read = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentCourses",
                columns: table => new
                {
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentCourses", x => new { x.StudentId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_StudentCourses_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_InstructorId1",
                table: "Groups",
                column: "InstructorId1");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourses_CourseId",
                table: "StudentCourses",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Courses_CourseId",
                table: "Assignments",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Courses_CourseId",
                table: "Groups",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Instructors_InstructorId1",
                table: "Groups",
                column: "InstructorId1",
                principalTable: "Instructors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_AspNetUsers_StudentId",
                table: "GroupStudents",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Courses_CourseId1",
                table: "Materials",
                column: "CourseId1",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Courses_CourseId1",
                table: "Quizzes",
                column: "CourseId1",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
