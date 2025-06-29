using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asasy.Persistence.Migrations
{
    public partial class EditSomeTbls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "NotifyUsers",
                newName: "AdsId");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "NotifyUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Show",
                table: "NotifyUsers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdsId",
                table: "NotifyUsers",
                newName: "OrderId");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "NotifyUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "Show",
                table: "NotifyUsers",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
