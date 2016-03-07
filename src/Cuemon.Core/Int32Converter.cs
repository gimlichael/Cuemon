using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cuemon
{
    public class Int32Converter
    {
        public static int FromHexadecimal(char digit)
        {
            if ('0' <= digit && digit <= '9') { return (int)(digit - '0'); }
            if ('a' <= digit && digit <= 'f') { return (int)(digit - 'a' + 10); }
            if ('A' <= digit && digit <= 'F') { return (int)(digit - 'A' + 10); }

            throw new ArgumentException(nameof(digit));
        }
    }
}