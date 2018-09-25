﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mobicon;

namespace Mobicon.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20180925130205_DeleteSegment")]
    partial class DeleteSegment
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Mobicon.Models.Config", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("SegmentId");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("UpdatedBy")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("SegmentId");

                    b.ToTable("Configs");
                });

            modelBuilder.Entity("Mobicon.Models.ConfigEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ConfigId");

                    b.Property<string>("Description");

                    b.Property<string>("EntryId")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Jira");

                    b.Property<string>("Key")
                        .IsRequired();

                    b.Property<int?>("SegmentPrefixFrom");

                    b.Property<int?>("SegmentPrefixTo");

                    b.Property<int>("Type");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.Property<int>("Version");

                    b.Property<DateTime>("VersionCreateTime");

                    b.Property<string>("VersionCreatedBy")
                        .IsRequired();

                    b.Property<string>("VersionPrefixFrom");

                    b.Property<string>("VersionPrefixTo");

                    b.HasKey("Id");

                    b.HasIndex("ConfigId");

                    b.ToTable("Entries");
                });

            modelBuilder.Entity("Mobicon.Models.EntryConfigSimplePrefix", b =>
                {
                    b.Property<int>("ConfigEntryId");

                    b.Property<int>("SimplePrefixId");

                    b.HasKey("ConfigEntryId", "SimplePrefixId");

                    b.HasIndex("SimplePrefixId");

                    b.ToTable("EntryConfigSimplePrefix");
                });

            modelBuilder.Entity("Mobicon.Models.Segment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Segments");
                });

            modelBuilder.Entity("Mobicon.Models.SimplePrefix", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("ParentId");

                    b.HasKey("Id");

                    b.ToTable("SimplePrefixes");

                    b.HasData(
                        new { Id = 1, Name = "W10" },
                        new { Id = 2, Name = "W10MOBILE" },
                        new { Id = 3, Name = "W10DESKTOP" },
                        new { Id = 4, Name = "IOS" },
                        new { Id = 5, Name = "IPHONE" },
                        new { Id = 6, Name = "IPAD" },
                        new { Id = 7, Name = "Android" },
                        new { Id = 8, Name = "Google" },
                        new { Id = 9, Name = "Amazon" },
                        new { Id = 10, Name = "MacOs" },
                        new { Id = 11, Name = "Win32" },
                        new { Id = 12, Name = "Web" },
                        new { Id = 13, Name = "BBCOM" },
                        new { Id = 14, Name = "FBCOM" },
                        new { Id = 15, Name = "DEV" },
                        new { Id = 16, Name = "GEN" },
                        new { Id = 17, Name = "PREVIEW" }
                    );
                });

            modelBuilder.Entity("Mobicon.Models.Snapshot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<string>("CreatedFrom");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime?>("PublishedAt");

                    b.Property<int>("Status");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("UpdatedBy")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Snapshots");
                });

            modelBuilder.Entity("Mobicon.Models.SnapshotApproval", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ApprovedAt");

                    b.Property<int>("SnapshotId");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("SnapshotId");

                    b.ToTable("SnapshotApprovals");
                });

            modelBuilder.Entity("Mobicon.Models.SnapshotToEntry", b =>
                {
                    b.Property<int>("EntryId");

                    b.Property<int>("SnapshotId");

                    b.HasKey("EntryId", "SnapshotId");

                    b.HasIndex("SnapshotId");

                    b.ToTable("SnapshotToEntry");
                });

            modelBuilder.Entity("Mobicon.Models.UserToRole", b =>
                {
                    b.Property<string>("Username")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Role");

                    b.HasKey("Username");

                    b.ToTable("UserRoles");

                    b.HasData(
                        new { Username = "grigoryp", Role = 5 },
                        new { Username = "yaroslavs", Role = 5 }
                    );
                });

            modelBuilder.Entity("Mobicon.Models.Config", b =>
                {
                    b.HasOne("Mobicon.Models.Segment", "Segment")
                        .WithMany("Configs")
                        .HasForeignKey("SegmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mobicon.Models.ConfigEntry", b =>
                {
                    b.HasOne("Mobicon.Models.Config", "Config")
                        .WithMany("Entries")
                        .HasForeignKey("ConfigId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mobicon.Models.EntryConfigSimplePrefix", b =>
                {
                    b.HasOne("Mobicon.Models.ConfigEntry", "ConfigEntry")
                        .WithMany("SimplePrefixes")
                        .HasForeignKey("ConfigEntryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mobicon.Models.SimplePrefix", "SimplePrefix")
                        .WithMany()
                        .HasForeignKey("SimplePrefixId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mobicon.Models.SnapshotApproval", b =>
                {
                    b.HasOne("Mobicon.Models.Snapshot", "Snapshot")
                        .WithMany("Approves")
                        .HasForeignKey("SnapshotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mobicon.Models.SnapshotToEntry", b =>
                {
                    b.HasOne("Mobicon.Models.ConfigEntry", "Entry")
                        .WithMany()
                        .HasForeignKey("EntryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mobicon.Models.Snapshot", "Snapshot")
                        .WithMany("Entries")
                        .HasForeignKey("SnapshotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
