using Microsoft.EntityFrameworkCore.Migrations;

namespace Mobicon.Migrations
{
    public partial class CreateDatabase102 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SnapshotToEntry_Entries_ConfigEntryId",
                table: "SnapshotToEntry");

            migrationBuilder.DropIndex(
                name: "IX_SnapshotToEntry_ConfigEntryId",
                table: "SnapshotToEntry");

            migrationBuilder.DropColumn(
                name: "ConfigEntryId",
                table: "SnapshotToEntry");

            migrationBuilder.AddForeignKey(
                name: "FK_SnapshotToEntry_Entries_EntryId",
                table: "SnapshotToEntry",
                column: "EntryId",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SnapshotToEntry_Entries_EntryId",
                table: "SnapshotToEntry");

            migrationBuilder.AddColumn<int>(
                name: "ConfigEntryId",
                table: "SnapshotToEntry",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotToEntry_ConfigEntryId",
                table: "SnapshotToEntry",
                column: "ConfigEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SnapshotToEntry_Entries_ConfigEntryId",
                table: "SnapshotToEntry",
                column: "ConfigEntryId",
                principalTable: "Entries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
