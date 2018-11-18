using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LetsChat
{
    class Packet
    {
        

        public const byte Request_Alive = 1;                  // 加入和存活信号
        public const byte Request_Hash_Check = 2;             // hash校验请求
        public const byte Request_Message = 3;                // 消息传送请求
        public const byte Request_Table = 4;                  // 更新成员表请求

        public const byte Response_Alive = 10;                 // 回应加入和存活信号
        public const byte Response_Hash_Check_Rrght = 11;      // 回应hash校验请求
        public const byte Response_Hash_Check_Error = 12;
        public const byte Response_Message = 13;               // 回应消息送到
        public const byte Response_Table = 14;                 // 回应TableData
                
        static byte[] String(string str)
        {
            MemoryStream ms = new MemoryStream();
            ms.WriteByte(Request_Message);
            byte[] te = Encoding.Default.GetBytes(str);
            ms.Write(Encoding.Default.GetBytes(str), 0, str.Length);
            te = ms.ToArray();
            return te;
        }
    }
}
