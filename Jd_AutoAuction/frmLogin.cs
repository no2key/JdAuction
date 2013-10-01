using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JdModule;
using System.Net;

namespace Jd_AutoAuction
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        public event EventHandler<JdResponseEventArgs> HttpRequestMessage;

        private void button1_Click(object sender, EventArgs e)
        {
            string msg = "";
            CookieCollection cookie = new CookieCollection();
            JdLogin jd = new JdLogin();
            bool success = jd.UserLogin(textBox1.Text, textBox2.Text, ref cookie, ref msg);
            if (HttpRequestMessage != null)
                HttpRequestMessage(this, new JdResponseEventArgs { ResponseMsg = msg, Cookie = cookie, LoginSuccess = success });
            this.Close();
        }
    }
}
