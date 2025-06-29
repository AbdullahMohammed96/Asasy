using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asasy.Persistence.Migrations
{
    public partial class EditTblReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Reports");
        }
    }
}
