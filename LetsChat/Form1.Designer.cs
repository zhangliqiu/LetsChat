namespace LetsChat
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listBox_Member = new System.Windows.Forms.ListBox();
            this.textBox_IP = new System.Windows.Forms.TextBox();
            this.textBox_PORT = new System.Windows.Forms.TextBox();
            this.button_Join = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBox_Member
            // 
            this.listBox_Member.FormattingEnabled = true;
            this.listBox_Member.ItemHeight = 12;
            this.listBox_Member.Location = new System.Drawing.Point(9, 62);
            this.listBox_Member.Name = "listBox_Member";
            this.listBox_Member.Size = new System.Drawing.Size(225, 400);
            this.listBox_Member.TabIndex = 0;
            this.listBox_Member.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox_Member_MouseDoubleClick);
            // 
            // textBox_IP
            // 
            this.textBox_IP.Location = new System.Drawing.Point(9, 6);
            this.textBox_IP.Name = "textBox_IP";
            this.textBox_IP.Size = new System.Drawing.Size(136, 21);
            this.textBox_IP.TabIndex = 0;
            // 
            // textBox_PORT
            // 
            this.textBox_PORT.Location = new System.Drawing.Point(171, 6);
            this.textBox_PORT.Name = "textBox_PORT";
            this.textBox_PORT.Size = new System.Drawing.Size(63, 21);
            this.textBox_PORT.TabIndex = 0;
            // 
            // button_Join
            // 
            this.button_Join.Location = new System.Drawing.Point(140, 33);
            this.button_Join.Name = "button_Join";
            this.button_Join.Size = new System.Drawing.Size(94, 23);
            this.button_Join.TabIndex = 1;
            this.button_Join.Text = "加入";
            this.button_Join.UseVisualStyleBackColor = true;
            this.button_Join.Click += new System.EventHandler(this.button_Join_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 465);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "label";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 496);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "label";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 517);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_IP);
            this.Controls.Add(this.button_Join);
            this.Controls.Add(this.textBox_PORT);
            this.Controls.Add(this.listBox_Member);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Let\'s Chatting";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Member;
        private System.Windows.Forms.TextBox textBox_PORT;
        private System.Windows.Forms.TextBox textBox_IP;
        private System.Windows.Forms.Button button_Join;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

