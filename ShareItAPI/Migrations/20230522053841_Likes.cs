using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareItAPI.Migrations
{
    public partial class Likes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Likes",
                table: "Posts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "CommentUser",
                columns: table => new
                {
                    LikedCommentsId = table.Column<long>(type: "bigint", nullable: false),
                    UserLikesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentUser", x => new { x.LikedCommentsId, x.UserLikesId });
                    table.ForeignKey(
                        name: "FK_CommentUser_Comments_LikedCommentsId",
                        column: x => x.LikedCommentsId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentUser_Users_UserLikesId",
                        column: x => x.UserLikesId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostUser1",
                columns: table => new
                {
                    LikedPostsId = table.Column<long>(type: "bigint", nullable: false),
                    UserLikesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostUser1", x => new { x.LikedPostsId, x.UserLikesId });
                    table.ForeignKey(
                        name: "FK_PostUser1_Posts_LikedPostsId",
                        column: x => x.LikedPostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostUser1_Users_UserLikesId",
                        column: x => x.UserLikesId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentUser_UserLikesId",
                table: "CommentUser",
                column: "UserLikesId");

            migrationBuilder.CreateIndex(
                name: "IX_PostUser1_UserLikesId",
                table: "PostUser1",
                column: "UserLikesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentUser");

            migrationBuilder.DropTable(
                name: "PostUser1");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Posts");
        }
    }
}
