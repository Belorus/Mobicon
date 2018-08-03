using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mobicon.Migrations
{
    public partial class CreateDatabase5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SnapshotId",
                table: "Entries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Snapshots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Snapshots", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_SnapshotId",
                table: "Entries",
                column: "SnapshotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Snapshots_SnapshotId",
                table: "Entries",
                column: "SnapshotId",
                principalTable: "Snapshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Snapshots_SnapshotId",
                table: "Entries");

            migrationBuilder.DropTable(
                name: "Snapshots");

            migrationBuilder.DropIndex(
                name: "IX_Entries_SnapshotId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "SnapshotId",
                table: "Entries");
        }
    }
}
