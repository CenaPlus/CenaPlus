using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Server.Bll
{
    public class ClientException : Exception
    {
        public ClientException() { }
        public ClientException(string msg) : base(msg) { }
    }
}
