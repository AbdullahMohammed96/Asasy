using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asasy.Persistence.Migrations
{
    public partial class EditTblSettingAddFooter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AdvertsmentDetails_AdsId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AspNetUsers_ProviderId",
                table: "Reports");

            migrationBuilder.AddColumn<string>(
                name: "FooterAr",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterEn",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "AdsId",
                table: "Reports",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AdvertsmentDetails_AdsId",
                table: "Reports",
                column: "AdsId",
                principalTable: "AdvertsmentDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_ProviderId",
                table: "Reports",
                column: "ProviderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AdvertsmentDetails_AdsId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AspNetUsers_ProviderId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "FooterAr",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "FooterEn",
                table: "Settings");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdsId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AdvertsmentDetails_AdsId",
                table: "Reports",
                column: "AdsId",
                principalTable: "AdvertsmentDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_ProviderId",
                table: "Reports",
                column: "ProviderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
