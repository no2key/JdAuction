using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HttpHelper;
using System.Text.RegularExpressions;

namespace JdModule
{
    public class JdLogin
    {
        public bool UserLogin(string userName, string passWord, ref CookieCollection loginCookie, ref string message)
        {
            CookieCollection cookies = new CookieCollection();
            HttpWebResponse http_response = null;
            string response_txt = "", name = "", value = "";

            http_response = Http.GetHttpResponse(JdUrls.LoginGetUrl, null, null, cookies);
            cookies = http_response.Cookies;
            response_txt = Http.ResponseString(http_response);

            FindHiddentInput(response_txt, ref name, ref value);

            if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(value))
            {
                IDictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("authcode", null);
                parameters.Add("loginname", userName);
                parameters.Add("loginpwd", passWord);
                parameters.Add("machineCpu", null);
                parameters.Add("machineDisk", null);
                parameters.Add("machineNet", null);
                parameters.Add("uuid", Guid.NewGuid().ToString());
                parameters.Add("nloginpwd", passWord);
                parameters.Add(name, value);

                http_response = Http.PostHttpResponse(JdUrls.LoginPostUrl, parameters, null, null, Encoding.GetEncoding("gb2312"), cookies);
                loginCookie = http_response.Cookies;

                message = Http.ResponseString(http_response);
                return Regex.IsMatch(message, "success", RegexOptions.IgnoreCase);
            }

            return false;
        }

        void FindHiddentInput(string input, ref string name, ref string value)
        {
            string pattern = "<input\\s+type=\"hidden\"\\s+name=\"?(?<name>[^\"]*)\"?\\s+value=\"?(?<value>[^\"]*)\" />";
            var m = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
            name = m.Groups["name"].Value;
            value = m.Groups["value"].Value;
        }
    }
}
