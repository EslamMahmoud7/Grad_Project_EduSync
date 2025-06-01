using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_AspNetUsers_CreatedByAdminId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "StudentProfileId",
                table: "Quizzes");

            migrationBuilder.RenameColumn(
                name: "SubmissionDate",
                table: "Quizzes",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "CreatedByAdminId",
                table: "Quizzes",
                newName: "InstructorId");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_CreatedByAdminId",
                table: "Quizzes",
                newName: "IX_Quizzes_InstructorId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Quizzes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Quizzes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Quizzes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Quizzes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxAttempts",
                table: "Quizzes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ShuffleQuestions",
                table: "Quizzes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "QuizModels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuizId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelIdentifier = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizModels_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuizModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_QuizModels_QuizModelId",
                        column: x => x.QuizModelId,
                        principalTable: "QuizModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentQuizAttempts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuizId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuizModelId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AttemptNumber = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Score = table.Column<double>(type: "float", nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentQuizAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentQuizAttempts_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentQuizAttempts_QuizModels_QuizModelId",
                        column: x => x.QuizModelId,
                        principalTable: "QuizModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentQuizAttempts_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionOptions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuestionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentQuizAnswers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentQuizAttemptId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuestionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SelectedOptionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: true),
                    PointsAwarded = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentQuizAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentQuizAnswers_QuestionOptions_SelectedOptionId",
                        column: x => x.SelectedOptionId,
                        principalTable: "QuestionOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentQuizAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentQuizAnswers_StudentQuizAttempts_StudentQuizAttemptId",
                        column: x => x.StudentQuizAttemptId,
                        principalTable: "StudentQuizAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionOptions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizModelId",
                table: "Questions",
                column: "QuizModelId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizModels_QuizId",
                table: "QuizModels",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizAnswers_QuestionId",
                table: "StudentQuizAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizAnswers_SelectedOptionId",
                table: "StudentQuizAnswers",
                column: "SelectedOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizAnswers_StudentQuizAttemptId",
                table: "StudentQuizAnswers",
                column: "StudentQuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizAttempts_QuizId",
                table: "StudentQuizAttempts",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizAttempts_QuizModelId",
                table: "StudentQuizAttempts",
                column: "QuizModelId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizAttempts_StudentId",
                table: "StudentQuizAttempts",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_AspNetUsers_InstructorId",
                table: "Quizzes",
                column: "InstructorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_AspNetUsers_InstructorId",
                table: "Quizzes");

            migrationBuilder.DropTable(
                name: "StudentQuizAnswers");

            migrationBuilder.DropTable(
                name: "QuestionOptions");

            migrationBuilder.DropTable(
                name: "StudentQuizAttempts");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "QuizModels");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "MaxAttempts",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "ShuffleQuestions",
                table: "Quizzes");

            migrationBuilder.RenameColumn(
                name: "InstructorId",
                table: "Quizzes",
                newName: "CreatedByAdminId");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Quizzes",
                newName: "SubmissionDate");

            migrationBuilder.RenameIndex(
                name: "IX_Quizzes_InstructorId",
                table: "Quizzes",
                newName: "IX_Quizzes_CreatedByAdminId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Quizzes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "Quizzes",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "StudentProfileId",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_AspNetUsers_CreatedByAdminId",
                table: "Quizzes",
                column: "CreatedByAdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
