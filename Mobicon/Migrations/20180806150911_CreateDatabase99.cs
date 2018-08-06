using Microsoft.EntityFrameworkCore.Migrations;

namespace Mobicon.Migrations
{
    public partial class CreateDatabase99 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Snapshots_SnapshotId",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_SnapshotId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "SnapshotId",
                table: "Entries");

            migrationBuilder.CreateTable(
                name: "SnapshotToEntry",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(nullable: false),
                    EntryId = table.Column<int>(nullable: false),
                    ConfigEntryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotToEntry", x => new { x.EntryId, x.SnapshotId });
                    table.ForeignKey(
                        name: "FK_SnapshotToEntry_Entries_ConfigEntryId",
                        column: x => x.ConfigEntryId,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SnapshotToEntry_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotToEntry_ConfigEntryId",
                table: "SnapshotToEntry",
                column: "ConfigEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotToEntry_SnapshotId",
                table: "SnapshotToEntry",
                column: "SnapshotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SnapshotToEntry");

            migrationBuilder.AddColumn<int>(
                name: "SnapshotId",
                table: "Entries",
                nullable: true);

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
    }
}
