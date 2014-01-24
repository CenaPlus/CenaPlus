using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
namespace TestContract
{
    [ProtoContract]
    public class Problem
    {
        [ProtoMember(1)]
        public Guid ID { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public string Content { get; set; }
    }
}
