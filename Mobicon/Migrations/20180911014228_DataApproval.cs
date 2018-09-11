using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mobicon.Migrations
{
    public partial class DataApproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SnapshotApprovals",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SnapshotId1 = table.Column<int>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    ApprovedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotApprovals", x => x.SnapshotId);
                    table.ForeignKey(
                        name: "FK_SnapshotApprovals_Snapshots_SnapshotId1",
                        column: x => x.SnapshotId1,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Username = table.Column<string>(nullable: false),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Username);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotApprovals_SnapshotId1",
                table: "SnapshotApprovals",
                column: "SnapshotId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SnapshotApprovals");

            migrationBuilder.DropTable(
                name: "UserRoles");
        }
    }
}
