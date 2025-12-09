using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakeryShopAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFacebookUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "WebsiteInfos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "WebsiteInfos");
        }
    }
}
