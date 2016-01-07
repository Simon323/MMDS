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
        public List<string> keyWordsList { get; set; }
        public List<Similar> similarOfferList { get; set; }

        public Offer(string offerId, List<string> keyWordsList)
        {
            this.offerId = offerId;
            this.keyWordsList = keyWordsList;
            this.similarOfferList = new List<Similar>();
        }

        public Offer(string offerId, List<string> keyWordsList, List<Similar> similarOfferList)
        {
            this.offerId = offerId;
            this.keyWordsList = keyWordsList;
            this.similarOfferList = similarOfferList;
        }
    }
}
