using Microsoft.EntityFrameworkCore.Migrations;

namespace MII_Media.Migrations
{
    public partial class UpdateCommentTable1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_ApplicationUserId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ApplicationUserId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "Commenter",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Commenter",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ApplicationUserId1",
                table: "Comments",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_ApplicationUserId1",
                table: "Comments",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
