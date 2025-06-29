using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asasy.Persistence.Migrations
{
    public partial class AddTblReplaiesComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReplaiesComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Replay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplaiesComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReplaiesComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReplaiesComments_CommentAds_CommentId",
                        column: x => x.CommentId,
                        principalTable: "CommentAds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReplaiesComments_CommentId",
                table: "ReplaiesComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplaiesComments_UserId",
                table: "ReplaiesComments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReplaiesComments");
        }
    }
}
