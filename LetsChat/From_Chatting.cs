using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LetsChat.Form1;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace LetsChat
{
    public partial class From_Chatting : Form
    {

        public bool bRecv = true;
        public bool bSend = false;

        //public ShowMessageService showMessageFromChild;
        //public UdpClient localudpclient;

        public ChildFromClose Fromclose;
        public IPEndPoint ChatWho;
        public bool UpdateLabel(string msg)
        {

            string strtime = DateTime.Now.ToString();
            string str = strtime + "\r\n接收： " + msg;

            richTextBox.Text += str + "\r\n";
            return true;
        }
        public From_Chatting()
        {
            InitializeComponent();

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter
                && textBox1.Text != ""
                )
            {

                string strtime = DateTime.Now.ToString();
                string str = strtime + "\r\n发送： " + textBox1.Text;

                richTextBox.Text += str + "\r\n";


                string strMes = textBox1.Text;
                byte[] mesbytes = Encoding.Default.GetBytes(strMes);
                MemoryStream ms = new MemoryStream();

                ms.WriteByte(Packet.Request_Message);
                ms.Write(mesbytes, 0, mesbytes.Length);
                mesbytes = ms.ToArray();

                localUdpclient.Send(mesbytes, mesbytes.Length, ChatWho);
                textBox1.Text = "";
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == System.Convert.ToChar(13))
            {
                e.Handled = true;
            }
        }

        private void From_Chatting_FormClosing(object sender, FormClosingEventArgs e)
        {
            Fromclose(ChatWho);
        }
    }
}
