using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Microsoft.Xna.Framework
{
    class BigfileEntry
    {
        public string Name { get; }
        public int Split { get; }
        public long Offset { get; }
        public long Length { get; }

        internal BigfileEntry(string name, int split, long offset, long length)
        {
            Name = name;
            Offset = offset;
            Split = split;
            Length = length;
        }
    }
}
