using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace LetsChat
{
    public partial class Form1 : Form
    {



        bool Table_Change_Permit = false;
        public Form1()
        {
            InitializeComponent();
        }

        // 此委托用于接受线程接受到消息后主窗口的相关响应
        public delegate void NetEvenCall(byte Type, byte[] pack, IPEndPoint ip_come);
        public event NetEvenCall net_even;

        // 此委托用于父窗口接收来自子窗口的发送消息相关响应
        public delegate bool ShowMessageService(string msg);

        // 此委托用于父窗口接收子窗口关闭的消息
        public delegate void ChildFromClose(IPEndPoint ipe_close);

        // 此委托用于DealComeTable函数更新显示列表用
        public delegate void UpdateListDis();
        public UpdateListDis disupdate;

        IPEndPoint localAddress = new IPEndPoint(IPAddress.Loopback, 0);

        static MemberTable MemberTable = new MemberTable();
        static client localclient = new client();
        private void Form1_Load(object sender, EventArgs e)
        {

            localUdpclient = new UdpClient(localAddress);
            Thread reth = new Thread(new ThreadStart(RecvTh));
            reth.IsBackground = true;
            reth.Start();

            net_even += new NetEvenCall(NetEven);
            //取得本地使用的接口信息
            localclient.Create((IPEndPoint)localUdpclient.Client.LocalEndPoint);

            //将自己加入Table
            localclient.hash_result = true;
            MemberTable.Add(localclient);
            TableToDis();

            disupdate = new UpdateListDis(UpdateDis);

            textBox_IP.Text = "127.0.0.1";
            timer1.Start();

            label1.Text = localclient.IP_PORT.ToString();
        }


        // 比较两个byte[]数组是否相同
        public bool BytesCompare(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return
            false;
            for (int i =
            0; i < b1.Length; i++)
                if (b1[i] != b2[i])
                    return
                    false;
            return true;
        }

        private void UpdateDis()// 更新显示表用
        {
            TableToDis();
        }

        private void Chat_Close(IPEndPoint ip_close)// 自窗口关闭后续处理
        {
            client client_close = new client();
            client_close.Create(ip_close);
            int index = Chatting_List.FindIndex(x => x._client.Long_ip == client_close.Long_ip);
            Chatting_List.RemoveAt(index);

        }
        private void NetEven(byte Type, byte[] pack, IPEndPoint ip_come)// 网络中转
        {
            switch (Type)
            {
                case Packet.Request_Alive:
                    CheckAlive(ip_come);
                    break;
                case Packet.Request_Message:
                    ChatControl(pack, ip_come);
                    break;
                default:
                    break;
            }
        }
        private void TableToDis()// 显示成员
        {
            listBox_Member.Items.Clear();
            foreach (var client in MemberTable.Tables)
            {
                listBox_Member.Items.Add(client.IP_PORT);
            }

            label2.Text = BitConverter.ToString(MemberTable.Hash_value);
        }

        private void button_Join_Click(object sender, EventArgs e)
        {
            string ip = textBox_IP.Text;
            string port = textBox_PORT.Text;
            int iport = int.Parse(port);
            IPEndPoint ip_to = new IPEndPoint(IPAddress.Parse(ip), iport);
            byte[] buff = { Packet.Request_Alive };
            localUdpclient.Send(buff, buff.Length, ip_to);
            Thread.Sleep(100);
            Table_Request(ip_to);

        }

        private void Table_Request(IPEndPoint iep)
        {
            byte[] buff = { Packet.Request_Table };
            localUdpclient.Send(buff, buff.Length, iep);
            Table_Change_Permit = true;

        }
        // Chatting 控制模块
        List<Chatting> Chatting_List = new List<Chatting>();
        public void ChatControl(byte[] pack, IPEndPoint ip_come)
        {

            CheckAlive(ip_come);
            client tem = new client();
            tem.Create(ip_come);

            // 搜索已经开启聊天的表
            int index = Chatting_List.FindIndex(x => x._client.Long_ip == tem.Long_ip);

            if (index == -1)  // 新用户
            {
                Chatting fm2 = new Chatting();
                fm2._client.Create(ip_come);

                fm2.fm2 = new From_Chatting();
                fm2.fm2.Text = ip_come.ToString();
                fm2.fm2.ChatWho = ip_come;
                fm2.fm2.Fromclose = new ChildFromClose(Chat_Close);
                fm2.fm2.Show();

                Chatting_List.Add(fm2);

                ShowMessageService sms = new ShowMessageService(fm2.fm2.UpdateLabel);
                byte[] str = pack.Skip(1).ToArray();
                this.BeginInvoke(sms, Encoding.Default.GetString(str));

            }
            else
            {
                ShowMessageService sms = new ShowMessageService(Chatting_List[index].fm2.UpdateLabel);
                byte[] str = pack.Skip(1).ToArray();
                this.BeginInvoke(sms, Encoding.Default.GetString(str));
            }

        }

        public static UdpClient localUdpclient;
        public static bool run = true;
        public void RecvTh()
        {
            IPEndPoint Remote = new IPEndPoint(IPAddress.Any, 0);

            while (run)
            {
                IAsyncResult iar = localUdpclient.BeginReceive(null, null);
                byte[] receiveData = localUdpclient.EndReceive(iar, ref Remote);

                AnalysisPacket(receiveData, Remote);


            }


        }


        // 解析网络封包
        private void AnalysisPacket(byte[] pack, IPEndPoint ip_come)
        {
            MemoryStream ms = new MemoryStream(pack, false);
            byte tm = (byte)(ms.ReadByte());
            switch (tm)
            {
                case Packet.Request_Message:
                case Packet.Request_Alive:
                    this.Invoke(net_even, tm, pack, ip_come);
                    break;
                case Packet.Request_Table:
                    RespondTable(ip_come);
                    break;
                case Packet.Response_Table:
                    this.Invoke(net_even, Packet.Response_Alive, pack, ip_come);
                    DealComeTable(pack.Skip(1).ToArray());
                    break;
                case Packet.Request_Hash_Check:
                    Check_Local_Hash(pack.Skip(1).ToArray(), ip_come);
                    this.Invoke(net_even, Packet.Response_Alive, pack, ip_come);
                    break;
                case Packet.Response_Hash_Check_Error:
                    Error_Result(false, ip_come);
                    this.Invoke(net_even, Packet.Response_Alive, pack, ip_come);
                    break;
                case Packet.Response_Hash_Check_Rrght:
                    Error_Result(true, ip_come);
                    this.Invoke(net_even, Packet.Response_Alive, pack, ip_come);
                    break;
                default:
                    break;
            }

        }
        private void Check_Local_Hash(byte[] hash_value, IPEndPoint iep)
        {
            bool result = BytesCompare(hash_value, MemberTable.Hash_value);
            if (result == true)
            {
                byte[] buff = { Packet.Response_Hash_Check_Rrght };
                localUdpclient.Send(buff, buff.Length, iep);
            }
            else
            {
                byte[] buff = { Packet.Response_Hash_Check_Error };
                localUdpclient.Send(buff, buff.Length, iep);

            }
        }

        private void Error_Result(bool status, IPEndPoint iep)
        {
            client cl = new client();
            cl.Create(iep);
            int index = MemberTable.Tables.FindIndex(x => x.Long_ip == cl.Long_ip);
            if (index >= 0)
                MemberTable.Tables[index].hash_result = status;
        }


        private void DealComeTable(byte[] TableBytes)
        {
            if (Table_Change_Permit == false)
                return;

            int n = TableBytes.Length / 8;

            MemberTable.Clear();

            client cl = new client();
            MemoryStream ms = new MemoryStream(TableBytes, false);
            byte[] clientbytes = new byte[8];

            for (int i = 0; i < n; i++)
            {
                ms.Read(clientbytes, 0, 8);
                cl.UnSerialize(clientbytes);
                MemberTable.Add(cl);

            }
            listBox_Member.BeginInvoke(disupdate);

            Table_Change_Permit = false;
        }

        #region 信息服务块

        // 回应Table 数据
        private void RespondTable(IPEndPoint iep)
        {
            MemoryStream ms = new MemoryStream();
            ms.WriteByte(Packet.Response_Table);
            ms.Write(MemberTable.Bytes, 0, MemberTable.Bytes.Length);
            byte[] buff = ms.ToArray();
            localUdpclient.Send(buff, buff.Length, iep);
        }

        #endregion
        private void CheckAlive(IPEndPoint ip_come)
        {
            client cl = new client();
            cl.Create(ip_come);
            if (MemberTable.Contains(cl))
            {
                int index = MemberTable.Tables.FindIndex(x => x.Long_ip == cl.Long_ip);
                MemberTable.Tables[index].boAlive = true;
                return;
            }
            cl.boAlive = true;
            MemberTable.Add(cl);
            TableToDis();
        }

        private void listBox_Member_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            IPEndPoint ipe = (IPEndPoint)listBox_Member.SelectedItem;
            client selectitem = new client();
            selectitem.Create(ipe);

            if (localclient.Long_ip == selectitem.Long_ip)
            {
                MessageBox.Show("您选择了与自己聊天");
                return;
            }

            int index = Chatting_List.FindIndex(x => x._client.Long_ip == selectitem.Long_ip);

            if (index == -1)
            {

                Chatting fm2 = new Chatting();
                fm2._client.Create(ipe);

                fm2.fm2 = new From_Chatting();
                fm2.fm2.Text = ipe.ToString();
                fm2.fm2.ChatWho = ipe;
                fm2.fm2.Fromclose = new ChildFromClose(Chat_Close);
                fm2.fm2.Show();

                Chatting_List.Add(fm2);
            }

        }

        private void SendToAll()
        {
            foreach (var cli in MemberTable.Tables)
                if (localclient.Long_ip != cli.Long_ip)
                {
                    SendHashTo(cli.IP_PORT);
                    Thread.Sleep(10);
                    SendAliveTo(cli.IP_PORT);
                }

        }

        private void SendHashTo(IPEndPoint iep)
        {
            MemoryStream ms = new MemoryStream();
            ms.WriteByte(Packet.Request_Hash_Check);
            ms.Write(MemberTable.Hash_value, 0, MemberTable.Hash_value.Length);
            byte[] buff = ms.ToArray();
            localUdpclient.Send(buff, buff.Length, iep);
        }

        private void SendAliveTo(IPEndPoint iep)
        {
            byte[] buff = { Packet.Request_Alive };
            localUdpclient.Send(buff, buff.Length, iep);
        }


        private void CheakAliveAndRemove()
        {
            for (int i = 0; i < MemberTable.Tables.Count; i++)
                if (MemberTable.Tables[i].boAlive == false
                    && MemberTable.Tables[i].Long_ip != localclient.Long_ip
                    )
                    MemberTable.Remove(MemberTable.Tables[i]);
            TableToDis();

        }
        private void UnAliveAll()
        {
            foreach (var cl in MemberTable.Tables)
            {
                if (cl.Long_ip == localclient.Long_ip)
                    continue;
                cl.boAlive = false;

            }
        }

        private void Result_Hash()
        {
            float i = 0;
            foreach (var cl in MemberTable.Tables)
            {
                if (cl.hash_result == false)
                {
                    i++;
                }
            }
            if (i >= (float)MemberTable.Count / 1.3)
            {
                foreach (var cl in MemberTable.Tables)
                    if (cl.hash_result == false)
                        Table_Request(cl.IP_PORT);


            }
        }
        int time = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            time++;
            SendToAll();  // 5 s

            if (time > 2)       // 15 s
            {
                UnAliveAll();               
                Result_Hash();
                if (time > 11)   //   60s
                {
                    CheakAliveAndRemove();
                    time = 0;
                }
            }
        }
    }
}
