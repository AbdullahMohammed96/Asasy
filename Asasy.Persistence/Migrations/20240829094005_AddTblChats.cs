using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asasy.Persistence.Migrations
{
    public partial class AddTblChats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_AspNetUsers_EmployeeId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Orders_OrderId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Messages",
                newName: "ChatId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_OrderId",
                table: "Messages",
                newName: "IX_Messages_ChatId");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "Complaints",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_AspNetUsers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chats_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_ProviderId",
                table: "Chats",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_UserId",
                table: "Chats",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_EmployeeId",
                table: "Complaints",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_AspNetUsers_EmployeeId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "Messages",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                newName: "IX_Messages_OrderId");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "Complaints",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_EmployeeId",
                table: "Complaints",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Orders_OrderId",
                table: "Messages",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
