using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsChat
{
    class Hash
    {
        static public byte[] Check(byte[]tbs)
        {
            var hash = System.Security.Cryptography.HashAlgorithm.Create();
            return hash.ComputeHash(tbs);
        }
    }
}
