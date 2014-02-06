using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
namespace CenaPlus.Entity
{
    [Table("test_cases")]
    public class TestCase
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("problem_id")]
        [ForeignKey("Problem")]
        public int ProblemID { get; set; }

        [IgnoreDataMember]
        public virtual Problem Problem { get; set; }

        [NotMapped]
        public string ProblemTitle { get; set; }

        [Column("type")]
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

        [Column("output")]
        public byte[] Output { get; set; }
    }

    public enum TestCaseType
    {
        Pretest, Systemtest
    }
}
