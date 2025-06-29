using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asasy.Persistence.Migrations
{
    public partial class EdiChatsAddAdId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdsId",
                table: "Chats",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chats_AdsId",
                table: "Chats",
                column: "AdsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_AdvertsmentDetails_AdsId",
                table: "Chats",
                column: "AdsId",
                principalTable: "AdvertsmentDetails",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_AdvertsmentDetails_AdsId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_AdsId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "AdsId",
                table: "Chats");
        }
    }
}
