using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Server.Bll
{
    public class AccessDeniedException:ClientException
    {
        public AccessDeniedException() { }
        public AccessDeniedException(string reason) : base(reason) { }
    }
}
