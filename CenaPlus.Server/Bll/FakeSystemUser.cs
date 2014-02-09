using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CenaPlus.Entity;

namespace CenaPlus.Server.Bll
{
    class FakeSystemUser : User
    {
        public FakeSystemUser()
        {
            this.ID = 0;
            this.Name = "System";
            this.NickName = "System";
            this.Password = null;
            this.Role = UserRole.System;
        }
    }
}
