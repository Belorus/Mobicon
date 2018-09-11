using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Mobicon.Models
{
    public enum UserRole
    {
        Reader,
        Editor,
        Approver,
        Admin
    }

    public class SnapshotApproval
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SnapshotId { get; set; }

        public Snapshot Snapshot { get; set; }

        public string Username { get; set; }

        public DateTime ApprovedAt { get; set; }
    }

    public class UserToRole
    {
        [Key]
        public string Username { get; set; }

        public UserRole Role { get; set; }
    }

    public class SnapshotToEntry
    {
        public int SnapshotId { get; set; }
        public Snapshot Snapshot { get; set; }

        public int EntryId { get; set; }
        public ConfigEntry Entry { get; set; }
    }

    public enum SnapshotStatus
    {
        WaitingForApprove,
        Published
    }

    public class Snapshot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public string UpdatedBy { get; set; }

        public SnapshotStatus Status { get; set; }

        public List<SnapshotToEntry> Entries { get; set; } = new List<SnapshotToEntry>();
    }

    public class Segment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public List<Config> Configs { get; set; } = new List<Config>();
    }

    public class Config
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int SegmentId { get; set; }
        public Segment Segment { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        public List<ConfigEntry> Entries { get; set; }
    }

    public class ConfigEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string EntryId { get; set; }

        public int Version { get; set; }

        public int ConfigId { get; set; }
        public Config Config { get; set; }        

        public string Key { get; set; }
        public string Value { get; set; }
        public FieldType Type { get; set; }
        public string Description { get; set; }
        public string Jira { get; set; }

        public DateTime VersionCreateTime { get; set; }
        public string VersionCreatedBy { get; set; }

        public int? SegmentPrefixFrom { get; set; }
        public int? SegmentPrefixTo{ get; set; }

        public string VersionPrefixFrom { get; set; }
        public string VersionPrefixTo { get; set; }

        public bool IsDeleted { get; set; }

        [JsonIgnore]
        public List<EntryConfigSimplePrefix> SimplePrefixes { get; set; }
    }

    public class EntryConfigSimplePrefix
    {
        public SimplePrefix SimplePrefix { get; set; }
        public int SimplePrefixId { get; set; }

        public int ConfigEntryId { get; set; }
        public ConfigEntry ConfigEntry { get; set; }
    }

    public class SimplePrefix
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }

    public enum FieldType
    {
        Unknown,
        String,
        Integer,
        Float,
        Bool,
        ListOfString,
        ListOfInteger,
        ListOfFloat,
        ListOfBool,
    }
}