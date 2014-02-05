using System;
using System.Collections.Generic;
using System.Text;

namespace CenaPlus.Judge
{
    public class Identity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public System.Security.SecureString secPassword
        {
            get
            {
                System.Security.SecureString pwd = new System.Security.SecureString();
                foreach (char c in Password)
                {
                    pwd.AppendChar(c);
                }
                return pwd;
            }
        }
    }
}
