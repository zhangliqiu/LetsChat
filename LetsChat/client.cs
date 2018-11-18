using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace LetsChat
{
    class client// 定义一个成员类
    {

        // 序列化后字节数组
        public byte[] SerializeBytes = new byte[BytesLength];
        // MemberTable 排序所用
        public ulong Long_ip;
        // 成员完整的地址结构                            
        public IPEndPoint IP_PORT = new IPEndPoint(IPAddress.Any, 0);
        // 序列化后长度
        static int BytesLength = 8;

        // 序列化，字节顺序转换临时变量
        byte[] hostBytes = new byte[4];

        // Hash 比对结果
        public bool hash_result=true;
        public bool boAlive = false;

        public void Create(IPEndPoint inIp)// 创建并序列化
        {
            IP_PORT.Address.Address = inIp.Address.Address;
            IP_PORT.Port = inIp.Port;

            MemoryStream ms = new MemoryStream(new byte[BytesLength]);

            // 放入端口
            ms.Write(BitConverter.GetBytes(inIp.Port), 0, sizeof(int));

            hostBytes = inIp.Address.GetAddressBytes();
            Array.Reverse(hostBytes);

            // 放入地址
            ms.Write(hostBytes, 0, hostBytes.Length);

            SerializeBytes = ms.ToArray();
            Long_ip = BitConverter.ToUInt64(SerializeBytes, 0);
        }

        public bool UnSerialize(byte[] bs)// 反序列化
        {
            if (bs.Length != BytesLength)
                return false;

            bs.CopyTo(SerializeBytes, 0);

            Long_ip = BitConverter.ToUInt64(SerializeBytes, 0);

            MemoryStream ms = new MemoryStream(SerializeBytes, false);

            // 取出端口
            ms.Read(hostBytes, 0, 4);
            IP_PORT.Port = BitConverter.ToInt32(hostBytes, 0);


            // 取出地址
            ms.Read(hostBytes, 0, 4);
            Array.Reverse(hostBytes);
            IP_PORT.Address.Address = BitConverter.ToUInt32(hostBytes, 0);


            return true;

        }

        public override bool Equals(object obj)// 重写相等函数
        {
            return (obj as client).Long_ip==this.Long_ip;
        }

    }
}
