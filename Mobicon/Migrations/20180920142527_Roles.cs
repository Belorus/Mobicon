using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mobicon.Migrations
{
    public partial class Roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

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

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "UserRoles",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserRoles",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "Id");

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "Role", "Username" },
                values: new object[,]
                {
                    { 1, 3, "grigoryp" },
                    { 2, 1, "grigoryp" },
                    { 3, 3, "yaroslavs" },
                    { 4, 1, "yaroslavs" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserRoles");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "UserRoles",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "Username");

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Username",
                keyValue: "grigoryp",
                column: "Role",
                value: 0);

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Username",
                keyValue: "yaroslavs",
                column: "Role",
                value: 0);

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Username", "Role" },
                values: new object[] { "alexeyra", 3 });
        }
    }
}
