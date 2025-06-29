using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asasy.Persistence.Migrations
{
    public partial class EditTblAsasyUserPackage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AdvertsmentDetails_AdsId",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_ProviderId",
                table: "Follows");

            migrationBuilder.AddColumn<int>(
                name: "CountDayes",
                table: "UserPackages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PackageId",
                table: "UserPackages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderId",
                table: "Follows",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "AdsId",
                table: "Follows",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AdvertsmentDetails_AdsId",
                table: "Follows",
                column: "AdsId",
                principalTable: "AdvertsmentDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_ProviderId",
                table: "Follows",
                column: "ProviderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AdvertsmentDetails_AdsId",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_ProviderId",
                table: "Follows");

            migrationBuilder.DropColumn(
                name: "CountDayes",
                table: "UserPackages");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "UserPackages");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderId",
                table: "Follows",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdsId",
                table: "Follows",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AdvertsmentDetails_AdsId",
                table: "Follows",
                column: "AdsId",
                principalTable: "AdvertsmentDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_ProviderId",
                table: "Follows",
                column: "ProviderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
