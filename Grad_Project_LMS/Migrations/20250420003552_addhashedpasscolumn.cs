using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grad_Project_LMS.Migrations
{
    /// <inheritdoc />
    public partial class addhashedpasscolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HashedPassword",
                table: "students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpires",
                table: "students",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashedPassword",
                table: "students");

            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "students");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpires",
                table: "students");
        }
    }
}
