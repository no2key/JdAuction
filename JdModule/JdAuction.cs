using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;

namespace JdModule
{
    public class JdAuction
    {
        CookieCollection g_Cookie = null;
        int g_BeforeSecond = 0;
        public event EventHandler<JdResponseEventArgs> RequestMessage;
        //private int bidlowestprice = 20;

        //public int Bidlowestprice
        //{
        //    get { return bidlowestprice; }
        //    set { bidlowestprice = value; }
        //}

        public JdAuction(CookieCollection cookie, int advanceSec)
        {
            g_Cookie = cookie;
            g_BeforeSecond = advanceSec * 1000;
        }

        public AuctionInfo GetAuctionInfo(string id)
        {
            var response = HttpHelper.Http.GetHttpResponse(JdUrls.ProductInfoUrl(id), null, null, null);
            string response_txt = HttpHelper.Http.ResponseString(response);

            AuctionInfo info = new AuctionInfo();
            info.Name = Regex.Match(response_txt, @"(?<=<title>)[\s\S]*?(?=【拍卖 价格】- 夺宝岛 - 京东商城 </title>)").Value;
            info.Used = Regex.Match(response_txt, "(?<=<li class=\"fore9\">)[\\s\\S]*(?=</li>[\\s]*<li class=\"fore5\">)").Value;

            double _time;
            if (double.TryParse(Regex.Match(response_txt, "(?<=,endTimeMili:)\\d{10}").Value, out _time))
                info.UnixTime = _time;

            decimal _jdprice;
            if (decimal.TryParse(Regex.Match(response_txt, "(?<=<li class=\"fore4\">京东价：<del>￥)[\\s\\S]*(?=</del></li>)").Value, out _jdprice))
                info.JdPrice = _jdprice;

            return info;
        }

        public decimal? GetLastPrice(string id)
        {
            var httpresponse = HttpHelper.Http.GetHttpResponse(JdUrls.AuctionInfoUrl(id), null, null, null);
            var response_txt = HttpHelper.Http.ResponseString(httpresponse);
            var _detial = JsonConvert.DeserializeObject<AuctionData>(response_txt);
            if (_detial.datas != null && 2 != _detial.auctionStatus)
                return _detial.datas[0].price;
            return null;
        }

        public string Bid(string id, decimal? nowPrice, bool debug = false)
        {
            string code = "000";

            if (debug == true)
                return "200";

            if (nowPrice.HasValue)
            {
                var httpresponse = HttpHelper.Http.GetHttpResponse(JdUrls.BidUrl(id, nowPrice.Value), null, null, g_Cookie);
                var response_txt = HttpHelper.Http.ResponseString(httpresponse);

                code = Regex.Match(response_txt, "(?<=\"code\":\")[\\d]*(?=\"})", RegexOptions.IgnoreCase).Value;
            }

            return code;
        }

        void SendTaskMsg(string msg)
        {
            if (RequestMessage != null)
                RequestMessage(this, new JdResponseEventArgs { ResponseMsg = msg });
        }

        public void AutoAuction(object auctionTask)
        {
            AuctionTask task = auctionTask as AuctionTask;
            string msg = "";
            string id = task.ID;
            var info = GetAuctionInfo(id);

            SendTaskMsg(CreateTaskMsg(task, info));

            TimeSpan _betweentime = info.LocalTime - DateTime.Now;
            int sleeptime = (int)_betweentime.TotalMilliseconds - g_BeforeSecond;

            if (sleeptime <= 0)
            {
                msg = CreateTaskMsg(id, "结束时间不正确或拍卖已结束", "错误");
            }
            else
            {
                SendTaskMsg(CreateTaskMsg(id, "等待竞拍……", "消息"));
                Thread.Sleep(sleeptime);
                decimal? lastprice = GetLastPrice(id);
                if (lastprice.HasValue)
                {
                    if (lastprice.Value > (task.Percent ? info.JdPrice * task.HighestPrice : task.HighestPrice))
                    {
                        msg = CreateTaskMsg(id, "最后价格 " + lastprice + " 高于上限", "取消任务");
                    }
                    else
                    {
                        decimal? bidprice = lastprice + task.BidPrice;
                        string code = Bid(id, lastprice + bidprice, true);
                        msg = CreateBidMsg(id, code, lastprice);
                    }
                }
                else
                {
                    msg = msg = CreateTaskMsg(id, "获取最后价格出错或拍卖已结束", "错误");
                }
            }

            SendTaskMsg(msg);
        }

        string CreateTaskMsg(string id, string msg, string title)
        {
            return String.Format("[{0}] ID:{1} {2}{3}", title, id, msg, Environment.NewLine);
        }

        string CreateBidMsg(string id, string code, decimal? price)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("-------------出价信息-------------" + Environment.NewLine);
            msg.Append("ID:\t" + id + Environment.NewLine);
            msg.Append("状态:\t" + code + Environment.NewLine);
            msg.Append("价格:\t" + price + Environment.NewLine);
            msg.Append("时间:\t" + DateTime.Now.ToString() + Environment.NewLine);
            msg.Append("消息:\t" + AuctionMsg.GetMsg(code) + Environment.NewLine);
            msg.Append("----------------------------------" + Environment.NewLine);
            return msg.ToString();
        }

        string CreateTaskMsg(AuctionTask task, AuctionInfo info)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("-------------任务信息-------------" + Environment.NewLine);
            msg.Append("ID:\t" + task.ID + Environment.NewLine);
            msg.Append("名称:\t" + info.Name + Environment.NewLine);
            msg.Append("使用状态:\t" + info.Used + Environment.NewLine);
            msg.Append("京东价格:\t" + info.JdPrice + Environment.NewLine);
            msg.AppendFormat("结束时间:\t{0} ({1}){2}", info.LocalTime, info.UnixTime, Environment.NewLine);
            msg.AppendFormat("上限价格:\t{0} ({1}){2}", task.Percent ? info.JdPrice * task.HighestPrice : task.HighestPrice, task.Percent ? "原价比例" : "自定义", Environment.NewLine);
            msg.Append("价格幅度:\t" + task.BidPrice + Environment.NewLine);
            msg.Append("----------------------------------" + Environment.NewLine);
            return msg.ToString();
        }
    }
}
