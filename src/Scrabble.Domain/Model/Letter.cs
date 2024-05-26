using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble.Domain.Model
{
    class Letter
    {
        public String name { get; set; }
        public short value { get; set; }

        public Letter(string name, short value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
