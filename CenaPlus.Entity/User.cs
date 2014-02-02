using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CenaPlus.Entity
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("nick_name")]
        public string NickName { get; set; }

        /// <summary>
        /// Stored in SHA-1
        /// </summary>
        [Column("password")]
        [IgnoreDataMember]
        public byte[] Password { get; set; }

        [Column("role")]
        public int RoleAsInt { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public UserRole Role
        {
            get { return (UserRole)RoleAsInt; }
            set { RoleAsInt = (int)value; }
        }

        [IgnoreDataMember]
        public virtual ICollection<Contest> AssignedContests { get; set; }

        [NotMapped]
        [IgnoreDataMember]
        public List<int> AssignedContestIDs
        {
            get { return AssignedContests.Select(c => c.ID).ToList(); }
        }
    }

    public enum UserRole
    {
        Banned, Competitor, Manager,
        /// <summary>
        /// Internal use only
        /// </summary>
        System
    }
}
