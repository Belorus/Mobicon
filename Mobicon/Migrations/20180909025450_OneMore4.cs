using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mobicon.Migrations
{
    public partial class OneMore4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_SegmentPrefixes_SegmentPrefixId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_VersionPrefixes_VersionPrefixId",
                table: "Entries");

            migrationBuilder.DropTable(
                name: "SegmentPrefixes");

            migrationBuilder.DropTable(
                name: "VersionPrefixes");

            migrationBuilder.DropIndex(
                name: "IX_Entries_SegmentPrefixId",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_VersionPrefixId",
                table: "Entries");

            migrationBuilder.RenameColumn(
                name: "VersionPrefixId",
                table: "Entries",
                newName: "SegmentPrefixTo");

            migrationBuilder.RenameColumn(
                name: "SegmentPrefixId",
                table: "Entries",
                newName: "SegmentPrefixFrom");

            migrationBuilder.AddColumn<string>(
                name: "VersionPrefixFrom",
                table: "Entries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VersionPrefixTo",
                table: "Entries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VersionPrefixFrom",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "VersionPrefixTo",
                table: "Entries");

            migrationBuilder.RenameColumn(
                name: "SegmentPrefixTo",
                table: "Entries",
                newName: "VersionPrefixId");

            migrationBuilder.RenameColumn(
                name: "SegmentPrefixFrom",
                table: "Entries",
                newName: "SegmentPrefixId");

            migrationBuilder.CreateTable(
                name: "SegmentPrefixes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    From = table.Column<int>(nullable: false),
                    To = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentPrefixes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VersionPrefixes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    From = table.Column<string>(nullable: true),
                    To = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersionPrefixes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_SegmentPrefixId",
                table: "Entries",
                column: "SegmentPrefixId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_VersionPrefixId",
                table: "Entries",
                column: "VersionPrefixId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_SegmentPrefixes_SegmentPrefixId",
                table: "Entries",
                column: "SegmentPrefixId",
                principalTable: "SegmentPrefixes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_VersionPrefixes_VersionPrefixId",
                table: "Entries",
                column: "VersionPrefixId",
                principalTable: "VersionPrefixes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
