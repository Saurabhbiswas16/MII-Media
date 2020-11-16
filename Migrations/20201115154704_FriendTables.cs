using Microsoft.EntityFrameworkCore.Migrations;

namespace MII_Media.Migrations
{
    public partial class FriendTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    FriendId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sent = table.Column<bool>(type: "bit", nullable: false),
                    Receive = table.Column<bool>(type: "bit", nullable: false),
                    Confirmed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => x.FriendId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friends");
        }
    }
}
