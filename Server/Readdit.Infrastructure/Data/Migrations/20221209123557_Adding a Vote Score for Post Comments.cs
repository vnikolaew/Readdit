using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Readdit.Infrastructure.Data.Migrations
{
    public partial class AddingaVoteScoreforPostComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoteScore",
                table: "PostComments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoteScore",
                table: "PostComments");
        }
    }
}
