using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mobicon.Migrations
{
    public partial class DataApproval3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SnapshotApprovals_Snapshots_SnapshotId1",
                table: "SnapshotApprovals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SnapshotApprovals",
                table: "SnapshotApprovals");

            migrationBuilder.DropIndex(
                name: "IX_SnapshotApprovals_SnapshotId1",
                table: "SnapshotApprovals");

            migrationBuilder.DropColumn(
                name: "SnapshotId1",
                table: "SnapshotApprovals");

            migrationBuilder.AlterColumn<int>(
                name: "SnapshotId",
                table: "SnapshotApprovals",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SnapshotApprovals",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SnapshotApprovals",
                table: "SnapshotApprovals",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotApprovals_SnapshotId",
                table: "SnapshotApprovals",
                column: "SnapshotId");

            migrationBuilder.AddForeignKey(
                name: "FK_SnapshotApprovals_Snapshots_SnapshotId",
                table: "SnapshotApprovals",
                column: "SnapshotId",
                principalTable: "Snapshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SnapshotApprovals_Snapshots_SnapshotId",
                table: "SnapshotApprovals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SnapshotApprovals",
                table: "SnapshotApprovals");

            migrationBuilder.DropIndex(
                name: "IX_SnapshotApprovals_SnapshotId",
                table: "SnapshotApprovals");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SnapshotApprovals");

            migrationBuilder.AlterColumn<int>(
                name: "SnapshotId",
                table: "SnapshotApprovals",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "SnapshotId1",
                table: "SnapshotApprovals",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SnapshotApprovals",
                table: "SnapshotApprovals",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotApprovals_SnapshotId1",
                table: "SnapshotApprovals",
                column: "SnapshotId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SnapshotApprovals_Snapshots_SnapshotId1",
                table: "SnapshotApprovals",
                column: "SnapshotId1",
                principalTable: "Snapshots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
