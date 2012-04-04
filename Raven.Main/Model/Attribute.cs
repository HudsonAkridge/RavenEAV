using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RavenDbTest.Model
{
    public class Attribute
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ValueTypes Type { get; set; }
    }
}
