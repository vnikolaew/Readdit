using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Readdit.Infrastructure.Data.Migrations
{
    public partial class AddingCountryCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_AspNetUsers_AdminId",
                table: "Communities");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Countries",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_AspNetUsers_AdminId",
                table: "Communities",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Communities_AspNetUsers_AdminId",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Countries");

            migrationBuilder.AddForeignKey(
                name: "FK_Communities_AspNetUsers_AdminId",
                table: "Communities",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
