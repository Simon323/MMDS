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

        public static Dictionary<int, List<int>> ReadArticleAndWorkers()
        {
            Dictionary<int, List<int>> articleDictionary = new Dictionary<int, List<int>>();
            Dictionary<int, string> authorsDictionary = ReadAllAuthors();
            Dictionary<string, int> workersDictionary = ReadAllWorkers();
            bool firstRow = true;
            using (FileStream fs = File.Open(@"C:\Users\Szymon-Acer\Documents\Ruby\put_prof_graph\scraping\tables\articleID_authorID.csv", FileMode.Open))
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

                    var values = s.Split('|');
                    int article = int.Parse(values[0].Trim());
                    int author = int.Parse(values[1].Trim());

                    if (!articleDictionary.ContainsKey(article))
                    {
                        var authorTemp = authorsDictionary[author];
                        if (workersDictionary.ContainsKey(authorTemp))
                        {
                            var workersList = new List<int>();
                            workersList.Add(workersDictionary[authorTemp]);
                            articleDictionary.Add(article, workersList);
                        }
                    }
                    else
                    {
                        var dictionaryItem = articleDictionary[article];

                        var authorTemp = authorsDictionary[author];
                        if (workersDictionary.ContainsKey(authorTemp))
                        {
                            var workerTemp = workersDictionary[authorTemp];

                            if (!dictionaryItem.Contains(workerTemp))
                                dictionaryItem.Add(workerTemp);
                        }
                    }
                }
            }

            return articleDictionary;
        }

        public static void GetArticlesOnlyHighFive()
        {
            Dictionary<int, List<int>> highFiveArticles = new Dictionary<int, List<int>>();
            Dictionary<int, List<int>> allArticlesDictionary = ReadArticleAndWorkers();
            bool firstRow = true;
            using (FileStream fs = File.Open(@"C:\Users\Szymon-Acer\Documents\Ruby\put_prof_graph\scraping\tables\articleID_workerID.csv", FileMode.Open))
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

                    var values = s.Split('|');
                    int worker = int.Parse(values[1].Trim());
                    int article = int.Parse(values[0].Trim());

                    //if (!worker.Equals(1) && !worker.Equals(9) && !worker.Equals(10) && !worker.Equals(11) && !worker.Equals(14))
                    //    continue;

                    //if (!worker.Equals(1))
                    //    continue;

                    if (!highFiveArticles.ContainsKey(worker))
                    {
                        var authorsList = new List<int>();
                        authorsList.Add(article);
                        highFiveArticles.Add(worker, authorsList);
                    }
                    else
                    {
                        var dictionaryItem = highFiveArticles[worker];

                        if (!dictionaryItem.Contains(article))
                            dictionaryItem.Add(article);
                    }
                }
            }

            List<Tuple<int, Dictionary<int, int>>> result = new List<Tuple<int, Dictionary<int, int>>>(); //worker i (lista workerów z którymi pisali i ile razem)

            foreach (var author in highFiveArticles.OrderBy(x => x.Key).ToList()) //iterowanie po workerze i jego artukułach
            {
                Dictionary<int, int> cooperateDectionary = GenerateResultDictionary();
                foreach (var article in author.Value) //iterowanie po artukułach
                {
                    if (allArticlesDictionary.ContainsKey(article)) //kto napisał artykuł
                    {
                        foreach(var x in allArticlesDictionary[article])
                        {
                            cooperateDectionary[x] += 1; //kto napisa ile artukułów z danym workerem
                        }
                    }
                }

                result.Add(new Tuple<int, Dictionary<int, int>>(author.Key, cooperateDectionary.OrderByDescending(x => x.Value).ToDictionary(y => y.Key, y => y.Value)));
            }

            SaveToFile(result);
            SaveToFileCooperateInformation(result);
        }

        public static Dictionary<int, string> ReadAllAuthors()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            bool firstRow = true;
            using (FileStream fs = File.Open(@"C:\Users\Szymon-Acer\Documents\Ruby\put_prof_graph\scraping\tables\author_googleid_table.csv", FileMode.Open))
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

                    var values = s.Split('|');
                    int authirId = int.Parse(values[0].Trim());
                    string googleId = values[2].Trim();

                    if (!result.ContainsKey(authirId))
                    {
                        result.Add(authirId, googleId);
                    }
                }
            }

            return result;
        }

        public static Dictionary<string, int> ReadAllWorkers()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            bool firstRow = true;
            using (FileStream fs = File.Open(@"C:\Users\Szymon-Acer\Documents\Ruby\put_prof_graph\scraping\tables\workers_table.csv", FileMode.Open))
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

                    var values = s.Split('|');
                    int workerId = int.Parse(values[0].Trim());
                    string googleId = values[1].Trim();

                    if (!result.ContainsKey(googleId))
                    {
                        result.Add(googleId, workerId);
                    }
                }
            }

            return result;
        }

        public static Dictionary<int, int> GenerateResultDictionary()
        {
            Dictionary<int, int> result = new Dictionary<int, int>();

            for(int i = 1; i < 99; i++)
            {
                result.Add(i, 0);
            }

            return result;
        }

        public static void SaveToFile(List<Tuple<int, Dictionary<int, int>>> result)
        {
            string stringResult = "";
            foreach (var item in result)
            {
                stringResult += "Worker: " + item.Item1;
                stringResult += Environment.NewLine;
                foreach (var x in item.Item2)
                {
                    stringResult += "Worker " + x.Key.ToString() + " Articles " + x.Value.ToString() + Environment.NewLine;
                }

                stringResult += Environment.NewLine;
                stringResult += "-----------------------------------------";
                stringResult += Environment.NewLine;
                stringResult += Environment.NewLine;
            }

            File.WriteAllText("resultFile.txt", stringResult);
        }

        public static void SaveToFileCooperateInformation(List<Tuple<int, Dictionary<int, int>>> result)
        {
            string stringResult = "";

            Dictionary<int, List<int>> dic = new Dictionary<int, List<int>>();
            for (int i = 1; i < 99; i++)
            {
                List<Tuple<int, int>> temp = new List<Tuple<int, int>>(); // worker z którym współpracuje i ile razem napisaliśmy
                foreach (var item in result)
                {
                    if(result.FirstOrDefault(x => x.Item1.Equals(i)) == null)
                    {
                        temp.Add(new Tuple<int, int>(0, 0));
                        continue; //gdy worker z nikim nie współpracuje
                    }

                    if(item.Item1 != i)
                    {
                        temp.Add(new Tuple<int, int>(item.Item1, item.Item2[i]));
                    }
                }

                var list = new List<int>();
                int iter = 0;
                int selectPosition = 1;
                foreach (var x in temp.OrderByDescending(x => x.Item2).Take(selectPosition))
                {
                    iter++;
                    if (iter < selectPosition)
                        continue;

                    list.Add(x.Item1);
                }

                dic.Add(i, list);
            }

            foreach (var x in dic)
            {
                foreach(var y in x.Value)
                {
                    stringResult += "Worker " + x.Key.ToString() + " Cooperate " + y.ToString() + Environment.NewLine;
                }

                stringResult += Environment.NewLine;
                stringResult += "-----------------------------------------";
                stringResult += Environment.NewLine;
                stringResult += Environment.NewLine;
            }

            File.WriteAllText("cooperation.txt", stringResult);
        }
    }
}
