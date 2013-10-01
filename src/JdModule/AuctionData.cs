using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdModule
{
    public class AuctionData
    {
        public AuctionLastInfo[] datas { get; set; }
        public int? pageNo { get; set; }
        public int? pageSize { get; set; }
        public int? totalItem { get; set; }
        public int? totalPages { get; set; }
        public string trxBuyerName { get; set; }
        public decimal? trxPrice { get; set; }
        public int? auctionStatus { get; set; }
    }

    public class AuctionLastInfo
    {
        public int id { get; set; }
        public int paimaiDealId { get; set; }
        public int productId { get; set; }
        public string userNickName { get; set; }
        public double? bidTime { get; set; }
        public decimal? price { get; set; }
        public string ipAddress { get; set; }
        public string bidStatus { get; set; }
    }
}
