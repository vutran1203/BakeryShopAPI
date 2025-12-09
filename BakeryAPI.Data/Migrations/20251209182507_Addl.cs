using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakeryShopAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SnowEffect",
                table: "WebsiteInfos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Theme",
                table: "WebsiteInfos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SnowEffect",
                table: "WebsiteInfos");

            migrationBuilder.DropColumn(
                name: "Theme",
                table: "WebsiteInfos");
        }
    }
}
