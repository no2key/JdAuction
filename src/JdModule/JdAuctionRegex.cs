using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdModule
{
    public class JdAuctionRegex
    {
        public const string Name = @"(?<=<title>)[\s\S]*?(?=【拍卖 价格】- 夺宝岛 - 京东商城 </title>)";
        public const string Used = "(?<=<li class=\"fore9\">)[\\s\\S]*(?=</li>[\\s]*<li class=\"fore5\">)";
        public const string UnixTime = "(?<=,endTimeMili:)\\d{10}";
        public const string JdPrice = "(?<=<li class=\"fore4\">京东价：<del>￥)[\\s\\S]*(?=</del></li>)";
        public const string Code = "(?<=\"code\":\")[\\d]*(?=\"})";
    }
}
