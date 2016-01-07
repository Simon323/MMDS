using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMDS.Application
{
    public class Similar
    {
        public string offerId { get; set; }
        public double percentSimilar { get; set; }

        public Similar(string offerId, double percentSimilar)
        {
            this.offerId = offerId;
            this.percentSimilar = percentSimilar;
        }
    }
}
