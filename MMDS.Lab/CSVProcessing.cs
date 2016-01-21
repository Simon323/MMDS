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
        public static Dictionary<int, HashSet<int>> ReadAllFactsToMemory()
        {
            bool firstRow = true;
            Dictionary<int, HashSet<int>> usersDictionary = new Dictionary<int, HashSet<int>>();
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
                        //var songList = new List<int>();
                        //songList.Add(song);
                        //Dictionary<int, int> x = new Dictionary<int, int>();
                        //x.Add(song, song);
                        HashSet<int> x = new HashSet<int>();
                        x.Add(song);
                        usersDictionary.Add(user, x);
                    }
                    else
                    {
                        var dictionaryItem = usersDictionary[user];
                        dictionaryItem.Add(song);

                        //if (!dictionaryItem.ContainsKey(song))
                        //    dictionaryItem.Add(song, song);
                    }
                }
            }

            watch.Stop();
            Console.Clear();
            Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss}", watch.Elapsed);

            return usersDictionary;
        }

        public static void ExportToFile(Dictionary<int, Dictionary<int, Double>> result)
        {
            string stringResult = "";

            foreach (var item in result)
            {
                stringResult += "User " + item.Key.ToString() + " :" + Environment.NewLine;

                foreach (var x in item.Value)
                {
                    stringResult += x.Key.ToString() + " " + x.Value.ToString() + Environment.NewLine;
                }
            }

            File.WriteAllText("resultFile.txt", stringResult);
        } 
    }
}
