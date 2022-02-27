using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CloudBruh.Trustartup.MediaSystem.Migrations
{
    public partial class RemoveMediaType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Media");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Media",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
