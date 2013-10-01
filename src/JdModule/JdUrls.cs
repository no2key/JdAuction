using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdModule
{

    public class JdUrls
    {
        public static readonly string LoginGetUrl = "https://passport.jd.com/new/login.aspx";
        public static readonly string LoginPostUrl = "https://passport.jd.com/uc/loginService";

        public static string ProductInfoUrl(string auctionId)
        {
            return "http://auction.jd.com/detail/" + auctionId;
        }

        public static string AuctionInfoUrl(string auctionId)
        {
            return "http://auction.jd.com/json/paimai/bid_records?&dealId=" + auctionId + "&pageNo=1&pageSize=1";
        }

        public static string BidUrl(string actionId, decimal nowPrice)
        {
            return "http://auction.jd.com/json/paimai/bid?dealId=" + actionId + "&price=" + (int)nowPrice;
        }
    }
}
