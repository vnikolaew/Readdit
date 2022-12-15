using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Readdit.Infrastructure.Data.Migrations
{
    public partial class AddingUserScoringModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCommunity_AspNetUsers_UserId",
                table: "UserCommunity");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCommunity_Communities_CommunityId",
                table: "UserCommunity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCommunity",
                table: "UserCommunity");

            migrationBuilder.RenameTable(
                name: "UserCommunity",
                newName: "UserCommunities");

            migrationBuilder.RenameIndex(
                name: "IX_UserCommunity_UserId",
                table: "UserCommunities",
                newName: "IX_UserCommunities_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCommunity_IsDeleted",
                table: "UserCommunities",
                newName: "IX_UserCommunities_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCommunities",
                table: "UserCommunities",
                columns: new[] { "CommunityId", "UserId" });

            migrationBuilder.CreateTable(
                name: "UserScores",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostsScore = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CommentsScore = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserScores_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UserCommunities_AspNetUsers_UserId",
                table: "UserCommunities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCommunities_Communities_CommunityId",
                table: "UserCommunities",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCommunities_AspNetUsers_UserId",
                table: "UserCommunities");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCommunities_Communities_CommunityId",
                table: "UserCommunities");

            migrationBuilder.DropTable(
                name: "UserScores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCommunities",
                table: "UserCommunities");

            migrationBuilder.RenameTable(
                name: "UserCommunities",
                newName: "UserCommunity");

            migrationBuilder.RenameIndex(
                name: "IX_UserCommunities_UserId",
                table: "UserCommunity",
                newName: "IX_UserCommunity_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCommunities_IsDeleted",
                table: "UserCommunity",
                newName: "IX_UserCommunity_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCommunity",
                table: "UserCommunity",
                columns: new[] { "CommunityId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserCommunity_AspNetUsers_UserId",
                table: "UserCommunity",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCommunity_Communities_CommunityId",
                table: "UserCommunity",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
