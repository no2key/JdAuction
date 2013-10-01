using System;
using System.Net;

namespace JdModule
{
    public class JdResponseEventArgs : EventArgs
    {
        public bool LoginSuccess { get; set; }
        public string ResponseMsg { get; set; }
        public CookieCollection Cookie { get; set; }
    }
}
