using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMDS.Lab
{
    public class CSVProcessing
    {
        public static Dictionary<int, List<int>> ReadAllFactsToMemory()
        {
            bool firstRow = true;
            Dictionary<int, List<int>> usersDictionary = new Dictionary<int, List<int>>();
            var watch = Stopwatch.StartNew();
            using (FileStream fs = File.Open(@"D:\Studia\MMDS\msdc-facts\facts.csv", FileMode.Open))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    if (firstRow)
                    {
                        firstRow = false;
                        continue;
                    }

                    var values = s.Split(',');
                    int user = int.Parse(values[1].Trim());
                    int song = int.Parse(values[0].Trim());

                    if (!usersDictionary.ContainsKey(user))
                    {
                        var songList = new List<int>();
                        songList.Add(song);
                        usersDictionary.Add(user, songList);
                    }
                    else
                    {
                        var dictionaryItem = usersDictionary[user];

                        if (!dictionaryItem.Contains(song))
                            dictionaryItem.Add(song);
                    }
                        

                    //factsList.Add(new Facts(int.Parse(values[0].Trim()), int.Parse(values[1].Trim()), int.Parse(values[2].Trim())));

                }
            }

            watch.Stop();
            Console.Clear();
            Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss}", watch.Elapsed);

            return usersDictionary;
        }
    }
}
