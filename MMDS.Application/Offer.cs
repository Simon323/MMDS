using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMDS.Application
{
    public class Offer
    {
        public string offerId { get; set; }
        public List<Similar> similarOfferList { get; set; } 

        public Offer(string offerId, List<Similar> similarOfferList)
        {
            this.offerId = offerId;
            this.similarOfferList = similarOfferList;
        }
    }
}
