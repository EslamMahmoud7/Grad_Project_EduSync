using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initmig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_AdminProfiles_AdminProfileId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_AdminProfileId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "AdminProfileId",
                table: "Announcements");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminProfileId",
                table: "Announcements",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_AdminProfileId",
                table: "Announcements",
                column: "AdminProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_AdminProfiles_AdminProfileId",
                table: "Announcements",
                column: "AdminProfileId",
                principalTable: "AdminProfiles",
                principalColumn: "Id");
        }
    }
}
