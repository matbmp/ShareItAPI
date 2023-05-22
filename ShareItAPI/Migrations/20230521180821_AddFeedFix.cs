using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareItAPI.Migrations
{
    public partial class AddFeedFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_UserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Posts");

            migrationBuilder.CreateTable(
                name: "PostUser",
                columns: table => new
                {
                    FeedPostsId = table.Column<int>(type: "integer", nullable: false),
                    UserFeedId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUser", x => new { x.FeedPostsId, x.UserFeedId });
                    table.ForeignKey(
                        name: "FK_PostUser_Posts_FeedPostsId",
                        column: x => x.FeedPostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUser_Users_UserFeedId",
                        column: x => x.UserFeedId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostUser_UserFeedId",
                table: "PostUser",
                column: "UserFeedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostUser");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Posts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
