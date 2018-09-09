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
    [Migration("20180909002142_OneMore")]
    partial class OneMore
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<int>("EntryId");

                    b.Property<string>("Jira");

                    b.Property<string>("Key");

                    b.Property<int?>("SegmentPrefixId");

                    b.Property<int>("Type");

                    b.Property<string>("Value");

                    b.Property<int>("Version");

                    b.Property<DateTime>("VersionCreateTime");

                    b.Property<string>("VersionCreatedBy");

                    b.Property<int?>("VersionPrefixId");

                    b.HasKey("Id");

                    b.HasIndex("ConfigId");

                    b.HasIndex("SegmentPrefixId");

                    b.HasIndex("VersionPrefixId");

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

            modelBuilder.Entity("Mobicon.Models.SegmentPrefix", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("From");

                    b.Property<int>("To");

                    b.HasKey("Id");

                    b.ToTable("SegmentPrefixes");
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

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("UpdatedBy")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Snapshots");
                });

            modelBuilder.Entity("Mobicon.Models.SnapshotToEntry", b =>
                {
                    b.Property<int>("EntryId");

                    b.Property<int>("SnapshotId");

                    b.HasKey("EntryId", "SnapshotId");

                    b.HasIndex("SnapshotId");

                    b.ToTable("SnapshotToEntry");
                });

            modelBuilder.Entity("Mobicon.Models.VersionPrefix", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("From");

                    b.Property<string>("To");

                    b.HasKey("Id");

                    b.ToTable("VersionPrefixes");
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

                    b.HasOne("Mobicon.Models.SegmentPrefix", "SegmentPrefix")
                        .WithMany("ConfigEntries")
                        .HasForeignKey("SegmentPrefixId");

                    b.HasOne("Mobicon.Models.VersionPrefix", "VersionPrefix")
                        .WithMany("ConfigEntries")
                        .HasForeignKey("VersionPrefixId");
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
