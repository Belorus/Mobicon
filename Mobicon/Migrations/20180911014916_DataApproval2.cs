using Microsoft.EntityFrameworkCore.Migrations;

namespace Mobicon.Migrations
{
    public partial class DataApproval2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Snapshots",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Username", "Role" },
                values: new object[] { "grigoryp", 3 });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Username", "Role" },
                values: new object[] { "yaroslavs", 3 });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Username", "Role" },
                values: new object[] { "alexeyra", 3 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Username",
                keyValue: "alexeyra");

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Username",
                keyValue: "grigoryp");

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Username",
                keyValue: "yaroslavs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Snapshots");
        }
    }
}
