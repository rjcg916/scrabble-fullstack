using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble.Domain.Model
{
    class Letter(string name, short value)
    {
        public String name { get; set; } = name;
        public short value { get; set; } = value;
    }
}
