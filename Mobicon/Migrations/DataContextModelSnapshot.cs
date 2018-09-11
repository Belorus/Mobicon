﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mobicon;

namespace Mobicon.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Mobicon.Models.Config", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Name");

                    b.Property<int>("SegmentId");

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

                    b.Property<string>("EntryId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Jira");

                    b.Property<string>("Key");

                    b.Property<int?>("SegmentPrefixFrom");

                    b.Property<int?>("SegmentPrefixTo");

                    b.Property<int>("Type");

                    b.Property<string>("Value");

                    b.Property<int>("Version");

                    b.Property<DateTime>("VersionCreateTime");

                    b.Property<string>("VersionCreatedBy");

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

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Segments");
                });

            modelBuilder.Entity("Mobicon.Models.SimplePrefix", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int?>("ParentId");

                    b.HasKey("Id");

                    b.ToTable("SimplePrefixes");
                });

            modelBuilder.Entity("Mobicon.Models.Snapshot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedBy")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

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

                    b.Property<string>("Username");

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
                        new { Username = "grigoryp", Role = 3 },
                        new { Username = "yaroslavs", Role = 3 },
                        new { Username = "alexeyra", Role = 3 }
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
                        .WithMany()
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
