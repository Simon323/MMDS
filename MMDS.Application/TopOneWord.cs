using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMDS.Application
{
    public class TopOneWord
    {
        public string word { get; set; }
        public int count { get; set; }

        public TopOneWord(string word, int count)
        {
            this.word = word;
            this.count = count;
        }
    }
}
