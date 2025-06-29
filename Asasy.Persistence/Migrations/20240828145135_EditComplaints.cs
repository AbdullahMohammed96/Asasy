using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asasy.Persistence.Migrations
{
    public partial class EditComplaints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Complaints",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "Complaints",
                type: "nvarchar(450)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_EmployeeId",
                table: "Complaints",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_AspNetUsers_EmployeeId",
                table: "Complaints",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_AspNetUsers_EmployeeId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_EmployeeId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Complaints");
        }
    }
}
