﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CenaPlus.Entity
{
    [Table("hacks")]
    public class Hack
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("record_id")]
        [ForeignKey("Record")]
        public int RecordID { get; set; }

        [IgnoreDataMember]
        public virtual Record Record { get; set; }

        [Column("generated_test_case_id")]
        public int? GeneratedTestCaseID { get; set; }

        [IgnoreDataMember]
        public virtual TestCase GeneratedTestCase { get; set; }

        [Column("hacker_id")]
        [ForeignKey("Hacker")]
        public int HackerID { get; set; }

        [IgnoreDataMember]
        public virtual User Hacker { get; set; }

        [NotMapped]
        public string HackerNickName { get; set; }

        [NotMapped]
        public int HackeeID { get; set; }

        [NotMapped]
        public string HackeeNickName { get; set; }

        [Column("status")]
        public int StatusAsInt { get; set; }

        [Column("time")]
        public DateTime Time { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public HackStatus Status
        {
            get { return (HackStatus)StatusAsInt; }
            set { StatusAsInt = (int)value; }
        }

        [Column("data_or_datamaker")]
        public string DataOrDatamaker { get; set; }

        [Column("datamaker_language")]
        public int? DatamakerLanguageAsInt { get; set; }

        /// <summary>
        /// the language of datamaker. null if a datamaker is not used.
        /// </summary>
        [NotMapped]
        [IgnoreDataMember]
        public ProgrammingLanguage? DatamakerLanguage
        {
            get { return (ProgrammingLanguage?)DatamakerLanguageAsInt; }
            set { DatamakerLanguageAsInt = (int?)value; }
        }

        [Column("detail")]
        public string Detail { get; set; }

        public override bool Equals(object obj)
        {
            Hack other = obj as Hack;
            if (other == null) return false;
            return other.ID == this.ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }

    public enum HackStatus
    {
        Pending, Running, Success, BadData, Failure, DatamakerError
    }
}
