using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace CenaPlus.Entity
{
    [Table("test_cases")]
    public class TestCase
    {
        [Column("id")]
        [ScriptIgnore]
        public int ID { get; set; }

        [Column("problem_id")]
        [ForeignKey("Problem")]
        [ScriptIgnore]
        public int ProblemID { get; set; }

        [IgnoreDataMember]
        [ScriptIgnore]
        public virtual Problem Problem { get; set; }

        [NotMapped]
        [ScriptIgnore]
        public string ProblemTitle { get; set; }

        [Column("type")]
        [ScriptIgnore]
        public int TypeAsInt { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public TestCaseType Type
        {
            get { return (TestCaseType)TypeAsInt; }
            set { TypeAsInt = (int)value; }
        }

        /// <summary>
        /// MD5 of input data
        /// </summary>
        [Column("input_hash")]
        public byte[] InputHash { get; set; }

        [Column("input")]
        public byte[] Input { get; set; }

        /// <summary>
        /// MD5 of output data
        /// </summary>
        [Column("output_hash")]
        public byte[] OutputHash { get; set; }

        [Column("output")]
        public byte[] Output { get; set; }

        [NotMapped]
        [ScriptIgnore]
        public string InputPreview { get; set; }

        [NotMapped]
        [ScriptIgnore]
        public string OutputPreview { get; set; }

        [NotMapped]
        [ScriptIgnore]
        public int InputSize { get; set; }

        [NotMapped]
        [ScriptIgnore]
        public int OutputSize { get; set; }

        public override bool Equals(object obj)
        {
            TestCase other = obj as TestCase;
            if (other == null) return false;
            return other.ID == this.ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }

    public enum TestCaseType
    {
        Pretest, Systemtest
    }
}
