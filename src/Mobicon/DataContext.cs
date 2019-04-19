using Microsoft.EntityFrameworkCore;
using Mobicon.Models;

namespace Mobicon
{
    public class DataContext : DbContext
    {
        public DbSet<Segment> Segments { get; set; }

        public DbSet<Snapshot> Snapshots { get; set; }

        public DbSet<Config> Configs { get; set; }

        public DbSet<ConfigEntry> Entries { get; set; }

        public DbSet<SimplePrefix> SimplePrefixes { get; set; }

        public DbSet<UserToRole> UserRoles { get; set; }

        public DbSet<SnapshotApproval> SnapshotApprovals { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SimplePrefix>()
                .HasData(
                    new SimplePrefix {Id = 1, Name = "W10"},
                    new SimplePrefix {Id = 2, Name = "W10MOBILE"},
                    new SimplePrefix {Id = 3, Name = "W10DESKTOP"},
                    new SimplePrefix {Id = 4, Name = "IOS"},
                    new SimplePrefix {Id = 5, Name = "IPHONE"},
                    new SimplePrefix {Id = 6, Name = "IPAD"},
                    new SimplePrefix {Id = 7, Name = "Android"},
                    new SimplePrefix {Id = 8, Name = "Google"},
                    new SimplePrefix {Id = 9, Name = "Amazon"},
                    new SimplePrefix {Id = 10, Name = "MacOs"},
                    new SimplePrefix {Id = 11, Name = "Win32"},
                    new SimplePrefix {Id = 12, Name = "Web"},
                    new SimplePrefix {Id = 13, Name = "BBCOM"},
                    new SimplePrefix {Id = 14, Name = "FBCOM"},
                    new SimplePrefix {Id = 15, Name = "DEV"},
                    new SimplePrefix {Id = 16, Name = "GEN"},
                    new SimplePrefix { Id = 17, Name = "PREVIEW" }
                );

            modelBuilder.Entity<SnapshotToEntry>().HasKey(e => new {e.EntryId, e.SnapshotId});


            modelBuilder.Entity<EntryConfigSimplePrefix>().HasKey(e => new {e.ConfigEntryId, e.SimplePrefixId});
            modelBuilder.Entity<EntryConfigSimplePrefix>()
                .HasOne<ConfigEntry>(e => e.ConfigEntry)
                .WithMany(s => s.SimplePrefixes)
                .HasForeignKey(s => s.ConfigEntryId);

            modelBuilder.Entity<EntryConfigSimplePrefix>()
                .HasOne<SimplePrefix>(e => e.SimplePrefix);

            modelBuilder.Entity<UserToRole>()
                .HasData(
                    new UserToRole {Username = "grigoryp", Role = UserRole.Editor | UserRole.Admin},
                    new UserToRole {Username = "yaroslavs", Role = UserRole.Editor | UserRole.Admin}
                );
        }
    }
}
