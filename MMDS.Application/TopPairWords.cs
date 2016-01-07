using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMDS.Application
{
    public class TopPairWords
    {
        public string wordOne { get; set; }
        public string wordTwo { get; set; }
        public int count { get; set; }

        public TopPairWords(string wordOne, string wordTwo, int count)
        {
            this.wordOne = wordOne;
            this.wordTwo = wordTwo;
            this.count = count;
        }
    }
}
