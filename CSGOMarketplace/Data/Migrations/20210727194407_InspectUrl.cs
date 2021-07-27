using Microsoft.EntityFrameworkCore.Migrations;

namespace CSGOMarketplace.Data.Migrations
{
    public partial class InspectUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InspectUrl",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InspectUrl",
                table: "Items");
        }
    }
}
