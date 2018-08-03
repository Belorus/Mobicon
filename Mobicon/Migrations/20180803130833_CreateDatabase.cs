using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mobicon.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Entries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<int>(nullable: false),
                    ConfigId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Jira = table.Column<string>(nullable: true),
                    VersionCreateTime = table.Column<DateTime>(nullable: false),
                    VersionCreatedBy = table.Column<string>(nullable: true),
                    SegmentPrefixId = table.Column<int>(nullable: true),
                    VersionPrefixId = table.Column<int>(nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Entries_SegmentPrefixes_SegmentPrefixId",
                        column: x => x.SegmentPrefixId,
                        principalTable: "SegmentPrefixes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entries_VersionPrefixes_VersionPrefixId",
                        column: x => x.VersionPrefixId,
                        principalTable: "VersionPrefixes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateIndex(
                name: "IX_Configs_SegmentId",
                table: "Configs",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_ConfigId",
                table: "Entries",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_SegmentPrefixId",
                table: "Entries",
                column: "SegmentPrefixId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_VersionPrefixId",
                table: "Entries",
                column: "VersionPrefixId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryConfigSimplePrefix_SimplePrefixId",
                table: "EntryConfigSimplePrefix",
                column: "SimplePrefixId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntryConfigSimplePrefix");

            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "SimplePrefixes");

            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "SegmentPrefixes");

            migrationBuilder.DropTable(
                name: "VersionPrefixes");

            migrationBuilder.DropTable(
                name: "Segments");
        }
    }
}
