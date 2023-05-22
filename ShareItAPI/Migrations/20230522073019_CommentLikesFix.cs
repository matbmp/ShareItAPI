using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareItAPI.Migrations
{
    public partial class CommentLikesFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentLike_Comments_CommentId",
                table: "CommentLike");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentLike_Users_UserId",
                table: "CommentLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentLike",
                table: "CommentLike");

            migrationBuilder.RenameTable(
                name: "CommentLike",
                newName: "CommentLikes");

            migrationBuilder.RenameIndex(
                name: "IX_CommentLike_UserId",
                table: "CommentLikes",
                newName: "IX_CommentLikes_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentLikes",
                table: "CommentLikes",
                columns: new[] { "CommentId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLikes_Comments_CommentId",
                table: "CommentLikes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLikes_Users_UserId",
                table: "CommentLikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentLikes_Comments_CommentId",
                table: "CommentLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentLikes_Users_UserId",
                table: "CommentLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentLikes",
                table: "CommentLikes");

            migrationBuilder.RenameTable(
                name: "CommentLikes",
                newName: "CommentLike");

            migrationBuilder.RenameIndex(
                name: "IX_CommentLikes_UserId",
                table: "CommentLike",
                newName: "IX_CommentLike_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentLike",
                table: "CommentLike",
                columns: new[] { "CommentId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLike_Comments_CommentId",
                table: "CommentLike",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLike_Users_UserId",
                table: "CommentLike",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
