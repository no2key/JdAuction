using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdModule
{
    public class AuctionTask
    {
        public string ID { get; set; }
        public bool Percent { get; set; }
        public decimal HighestPrice { get; set; }
        public int BidPrice { get; set; }
    }
}
