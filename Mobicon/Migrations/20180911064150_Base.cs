using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mobicon.Migrations
{
    public partial class Base : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Segments",
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
                    table.PrimaryKey("PK_Segments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SimplePrefixes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimplePrefixes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Snapshots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Snapshots", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    SegmentId = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configs_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SnapshotApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SnapshotId = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    ApprovedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SnapshotApprovals_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EntryId = table.Column<string>(nullable: true),
                    Version = table.Column<int>(nullable: false),
                    ConfigId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Jira = table.Column<string>(nullable: true),
                    VersionCreateTime = table.Column<DateTime>(nullable: false),
                    VersionCreatedBy = table.Column<string>(nullable: true),
                    SegmentPrefixFrom = table.Column<int>(nullable: true),
                    SegmentPrefixTo = table.Column<int>(nullable: true),
                    VersionPrefixFrom = table.Column<string>(nullable: true),
                    VersionPrefixTo = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entries_Configs_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "Configs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryConfigSimplePrefix",
                columns: table => new
                {
                    SimplePrefixId = table.Column<int>(nullable: false),
                    ConfigEntryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryConfigSimplePrefix", x => new { x.ConfigEntryId, x.SimplePrefixId });
                    table.ForeignKey(
                        name: "FK_EntryConfigSimplePrefix_Entries_ConfigEntryId",
                        column: x => x.ConfigEntryId,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntryConfigSimplePrefix_SimplePrefixes_SimplePrefixId",
                        column: x => x.SimplePrefixId,
                        principalTable: "SimplePrefixes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SnapshotToEntry",
                columns: table => new
                {
                    SnapshotId = table.Column<int>(nullable: false),
                    EntryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotToEntry", x => new { x.EntryId, x.SnapshotId });
                    table.ForeignKey(
                        name: "FK_SnapshotToEntry_Entries_EntryId",
                        column: x => x.EntryId,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SnapshotToEntry_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SimplePrefixes",
                columns: new[] { "Id", "Description", "Name", "ParentId" },
                values: new object[,]
                {
                    { 1, null, "W10", null },
                    { 17, null, "PREVIEW", null },
                    { 16, null, "GEN", null },
                    { 15, null, "DEV", null },
                    { 14, null, "FBCOM", null },
                    { 13, null, "BBCOM", null },
                    { 12, null, "Web", null },
                    { 11, null, "Win32", null },
                    { 10, null, "MacOs", null },
                    { 8, null, "Google", null },
                    { 7, null, "Android", null },
                    { 6, null, "IPAD", null },
                    { 5, null, "IPHONE", null },
                    { 4, null, "IOS", null },
                    { 3, null, "W10DESKTOP", null },
                    { 2, null, "W10MOBILE", null },
                    { 9, null, "Amazon", null }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Username", "Role" },
                values: new object[,]
                {
                    { "yaroslavs", 3 },
                    { "grigoryp", 3 },
                    { "alexeyra", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configs_SegmentId",
                table: "Configs",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_ConfigId",
                table: "Entries",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryConfigSimplePrefix_SimplePrefixId",
                table: "EntryConfigSimplePrefix",
                column: "SimplePrefixId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotApprovals_SnapshotId",
                table: "SnapshotApprovals",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotToEntry_SnapshotId",
                table: "SnapshotToEntry",
                column: "SnapshotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntryConfigSimplePrefix");

            migrationBuilder.DropTable(
                name: "SnapshotApprovals");

            migrationBuilder.DropTable(
                name: "SnapshotToEntry");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "SimplePrefixes");

            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "Snapshots");

            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "Segments");
        }
    }
}
