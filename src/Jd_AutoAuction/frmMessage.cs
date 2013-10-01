using JdModule;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jd_AutoAuction
{
    public partial class frmMessage : Form
    {
        CookieCollection cookie = null;
        bool BidDebug = false;
        int Second = 5;

        public frmMessage()
        {
            InitializeComponent();
            cookie = new CookieCollection();
        }

        private void frmMessage_Load(object sender, EventArgs e)
        {
            string _sec = ConfigurationManager.AppSettings["AdvanceSecond"];
            if (!String.IsNullOrEmpty(_sec))
                int.TryParse(_sec, out Second);

            string _debug = ConfigurationManager.AppSettings["Debug"];
            if (!String.IsNullOrEmpty(_debug))
                bool.TryParse(_debug, out BidDebug);

            if (BidDebug)
                richTextBox1.AppendText("-->DEBUG" + Environment.NewLine);

            if (cookie.Count == 0)
            {
                frmLogin login = new frmLogin();
                login.HttpRequestMessage += LoginMessage;
                login.ShowDialog();
            }
        }

        void LoginMessage(object sender, JdModule.JdResponseEventArgs e)
        {
            cookie = e.Cookie;
            toolStripStatusLabel1.Text = e.LoginSuccess ? "登录成功" : "登录失败";
        }


        void InfoMessage(object sender, JdModule.JdResponseEventArgs e)
        {
            Action<string> AppentText = (msg) => richTextBox1.AppendText(msg);

            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(AppentText, new object[] { e.ResponseMsg + Environment.NewLine });
            }
            else
            {
                richTextBox1.AppendText(e.ResponseMsg + Environment.NewLine);
                richTextBox1.ScrollToCaret();
            }
        }

        void TxtAppent(string str)
        {
            richTextBox1.AppendText(str + Environment.NewLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            decimal _highest;
            int _bidprice;

            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("拍卖ID不能为空");
                return;
            }

            if (String.IsNullOrEmpty(textBox2.Text) || !decimal.TryParse(textBox2.Text, out _highest))
            {
                MessageBox.Show("上限价格格式不正确");
                return;
            }

            if (String.IsNullOrEmpty(textBox3.Text) || !int.TryParse(textBox3.Text, out _bidprice))
            {
                MessageBox.Show("价格幅度格式必须是正整数");
                return;
            }

            JdAuction bid = new JdAuction(cookie, Second, BidDebug);
            bid.RequestMessage += InfoMessage;

            var auction = textBox1.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string id in auction)
            {
                AuctionTask task = new AuctionTask
                {
                    Percent = radioButton1.Checked == true,
                    ID = id,
                    BidPrice = _bidprice,
                    HighestPrice = _highest
                };
                ThreadPool.QueueUserWorkItem(new WaitCallback(bid.AutoAuction), task);
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as System.Windows.Forms.RadioButton).Checked)
                textBox2.Text = string.Empty;
        }
    }
}
