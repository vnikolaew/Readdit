using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Readdit.Infrastructure.Data.Migrations
{
    public partial class MovingCountryandGenderpropertiestotheProfileentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Countries_CountryId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CountryId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                table: "UserProfiles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "0b8374c5-1387-4729-9ed6-5f34c91dc5a9");

            migrationBuilder.AddColumn<short>(
                name: "Gender",
                table: "UserProfiles",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CountryId",
                table: "UserProfiles",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Countries_CountryId",
                table: "UserProfiles",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Countries_CountryId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_CountryId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "UserProfiles");

            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "Gender",
                table: "AspNetUsers",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CountryId",
                table: "AspNetUsers",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Countries_CountryId",
                table: "AspNetUsers",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
