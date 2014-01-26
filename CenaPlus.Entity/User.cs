using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace CenaPlus.Entity
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("name")]
        public string Name { get; set; }

        /// <summary>
        /// Stored in SHA-1
        /// </summary>
        [Column("password")]
        public byte[] Password { get; set; }

        [Column("role")]
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Competitor, Manager
    }
}
