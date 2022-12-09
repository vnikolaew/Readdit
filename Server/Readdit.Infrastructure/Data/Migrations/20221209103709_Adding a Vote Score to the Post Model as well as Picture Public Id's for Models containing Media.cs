using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Readdit.Infrastructure.Data.Migrations
{
    public partial class AddingaVoteScoretothePostModelaswellasPicturePublicIdsforModelscontainingMedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePublicId",
                table: "UserProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MediaPublicId",
                table: "CommunityPosts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "VoteScore",
                table: "CommunityPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PicturePublicId",
                table: "Communities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicturePublicId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "MediaPublicId",
                table: "CommunityPosts");

            migrationBuilder.DropColumn(
                name: "VoteScore",
                table: "CommunityPosts");

            migrationBuilder.DropColumn(
                name: "PicturePublicId",
                table: "Communities");
        }
    }
}
