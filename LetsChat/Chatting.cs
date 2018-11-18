using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsChat
{
    class Chatting
    {
        public client _client = new client();
        public From_Chatting fm2;

        public override bool Equals(object obj)// 重写相等函数
        {
            return (obj as Chatting)._client==this._client;
        }
    }
}
