﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mobicon.Models
{
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

        public List<SegmentPrefix> SegmentPrefixes { get; set; }
        public List<VersionPrefix> VersionPrefixes { get; set; }
        public List<EntryConfigSimplePrefix> SimplePrefixes { get; set; }
    }

    public class EntryConfigSimplePrefix
    {
        public SimplePrefix SimplePrefix { get; set; }
        public int SimplePrefixId { get; set; }

        public int ConfigEntryId { get; set; }
        public ConfigEntry ConfigEntry { get; set; }
    }

    public class VersionPrefix
    {
        [Key]
        [ForeignKey("ConfigEntry")]
        public int ConfigEntryId { get; set; }
        public ConfigEntry ConfigEntry { get; set; }

        public string From { get; set; }
        public string To { get; set; }
    }

    public class SegmentPrefix
    {
        [Key]
        [ForeignKey("ConfigEntry")]
        public int ConfigEntryId { get; set; }
        public ConfigEntry ConfigEntry { get; set; }

        public int From { get; set; }
        public int To { get; set; }
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