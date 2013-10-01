using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdModule
{
    public class AuctionInfo
    {
        public string Name { get; set; }
        public string Used { get; set; }

        public double UnixTime { get; set; }
        public decimal JdPrice { get; set; }

        public DateTime LocalTime
        {
            get
            {
                if (UnixTime == 0)
                    return DateTime.Now;
                return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(UnixTime);
            }
        }
    }
}
