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
    
   

    // 定义一个成员表类
    class MemberTable
    {
        public byte[] Hash_value = new byte[20];      // 存放 hash 值   
        public byte[] Bytes = null;                   // 序列化最终值
        public int Count;

        List<byte> Tbytes = new List<byte>();         // 序列化工具
        public  List<client> Tables = new List<client>();     // 成员表
               
    
        public bool Contains(client cl)// 检查成员是否已存在
        {
            return Tables.Contains(cl);
        }
        public bool Add(client cl)// 添加成员
        {
            if (Tables.Contains(cl)) return false;

            client cli = new client();
            cli.Create(cl.IP_PORT);
            Tables.Add(cli);
            Update();
            return true;

        }
        public bool Remove(client cl)// 删除成员
        {

            if (!Tables.Contains(cl)) return false;

            Tables.Remove(cl);
            Update();
            return true;
        }

        public void Clear()
        {
            Tables.Clear();
            Update();
        }
        private void Update()
        {

            Count = Tables.Count;
            Tables.Sort(SortMath);
            ToBytes();
            Hash_value = Hash.Check(Bytes);
            
        }

        private int SortMath(client c1, client c2)// 排序规则
        {
            if (c1.Long_ip > c2.Long_ip)
                return 1;
            else if (c1.Long_ip < c2.Long_ip)
                return -1;
            else
                return 0;

        }
        private void ToBytes()//序列化
        {
            Tbytes.Clear();
            foreach (var client in Tables)
                Tbytes.AddRange(client.SerializeBytes);
            Bytes = Tbytes.ToArray();
        }
    }


}
