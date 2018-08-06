using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mobicon.Migrations
{
    public partial class CreateDatabase100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Snapshots",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Snapshots",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Snapshots");
        }
    }
}
