using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace CenaPlus.Cloud.Entity
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("gravatar")]
        public string Gravatar { get; set; }

        [Column("real_name")]
        public string RealName { get; set; }

        [Column("identification_number")]
        public string IdentificationNumber { get; set; }

        [Column("nick_name")]
        public string NickName { get; set; }

        /// <summary>
        /// Stored in SHA-1
        /// </summary>
        [Column("password")]
        public byte[] Password { get; set; }

        [Column("role")]
        public int RoleAsInt { get; set; }

        [NotMapped]
        public UserRole Role
        {
            get { return (UserRole)RoleAsInt; }
            set { RoleAsInt = (int)value; }
        }

        public virtual ICollection<Rating> Ratings { get; set; }

        public override bool Equals(object obj)
        {
            User other = obj as User;
            if (other == null) return false;
            return other.ID == this.ID;
        }

        public string GetGravatar()
        {
            var email_hash = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Gravatar, "MD5").ToLower();
            return String.Format("http://www.gravatar.com/avatar/{0}?d=http://www.cenaplus.org/images/Default.png", Gravatar);
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }

    public enum UserRole
    {
        Banned, User, Customer, Manager, Root
    }
}
