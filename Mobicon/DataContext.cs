using Microsoft.EntityFrameworkCore;
using Mobicon.Models;

namespace Mobicon
{
    public class DataContext : DbContext
    {
        public DbSet<Segment> Segments { get; set; }

        public DbSet<Config> Configs { get; set; }

        public DbSet<ConfigEntry> Entries { get; set; }

        public DbSet<SimplePrefix> SimplePrefixes { get; set; }

        public DbSet<VersionPrefix> VersionPrefixes { get; set; }

        public DbSet<SegmentPrefix> SegmentPrefixes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(@"Server=localhost;Database=moco;Uid=root;Pwd=Ыутвьщку1");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntryConfigSimplePrefix>().HasKey(e => new {e.ConfigEntryId, e.SimplePrefixId});
            modelBuilder.Entity<EntryConfigSimplePrefix>()
                .HasOne<ConfigEntry>(e => e.ConfigEntry)
                .WithMany(s => s.SimplePrefixes)
                .HasForeignKey(s => s.ConfigEntryId);

            modelBuilder.Entity<EntryConfigSimplePrefix>()
                .HasOne<SimplePrefix>(e => e.SimplePrefix);
        }
    }
}
