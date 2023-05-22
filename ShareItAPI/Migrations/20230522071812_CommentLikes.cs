using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareItAPI.Migrations
{
    public partial class CommentLikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentUser");

            migrationBuilder.CreateTable(
                name: "CommentLike",
                columns: table => new
                {
                    CommentId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentLike", x => new { x.CommentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CommentLike_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentLike_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentLike_UserId",
                table: "CommentLike",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentLike");

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

            migrationBuilder.CreateIndex(
                name: "IX_CommentUser_UserLikesId",
                table: "CommentUser",
                column: "UserLikesId");
        }
    }
}
