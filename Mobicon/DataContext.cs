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
                    new UserToRole {Username = "grigoryp", Role = UserRole.Admin},
                    new UserToRole {Username = "yaroslavs", Role = UserRole.Admin},
                    new UserToRole {Username = "alexeyra", Role = UserRole.Admin}
                    );
        }
    }
}
