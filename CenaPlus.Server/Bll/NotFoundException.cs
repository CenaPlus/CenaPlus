﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenaPlus.Server.Bll
{
    public class NotFoundException:Exception
    {
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
    }
}
