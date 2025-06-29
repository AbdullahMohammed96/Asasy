using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asasy.Persistence.Migrations
{
    public partial class EditTblSettingVTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactUsAr",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactUsEn",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiscountSystemAr",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiscountSystemEn",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PolicyAr",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PolicyEn",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactUsAr",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "ContactUsEn",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "DiscountSystemAr",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "DiscountSystemEn",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PolicyAr",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PolicyEn",
                table: "Settings");
        }
    }
}
