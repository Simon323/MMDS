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

            List<Dictionary<int, int>> result = new List<Dictionary<int, int>>();

            foreach (var author in highFiveArticles)
            {
                Dictionary<int, int> cooperateDectionary = GenerateResultDictionary();
                foreach (var article in author.Value)
                {
                    if (allArticlesDictionary.ContainsKey(article))
                    {
                        foreach(var x in allArticlesDictionary[article])
                        {
                            cooperateDectionary[x] += 1;
                        }
                    }
                }

                result.Add(cooperateDectionary.OrderByDescending(x => x.Value).ToDictionary(y => y.Key, y => y.Value));
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

        public static void SaveToFile(List<Dictionary<int, int>> result)
        {
            string stringResult = "";
            int iter = 0;
            foreach (var item in result)
            {
                iter++;
                stringResult += "Worker: " + iter;
                stringResult += Environment.NewLine;
                foreach (var x in item)
                {
                    stringResult += "Worker " + x.Key.ToString() + " Articles" + x.Value.ToString() + Environment.NewLine;
                }

                stringResult += Environment.NewLine;
                stringResult += "-----------------------------------------";
                stringResult += Environment.NewLine;
                stringResult += Environment.NewLine;
            }

            File.WriteAllText("resultFile.txt", stringResult);
        }

        public static void SaveToFileCooperateInformation(List<Dictionary<int, int>> result)
        {
            string stringResult = "";

            Dictionary<int, int> dic = new Dictionary<int, int>();
            for (int i = 1; i < 99; i++)
            {
                int worker = 0;
                int ascribeToWorker = 0;
                int bestWrittingArticleCounter = 0;
                foreach (var item in result)
                {
                    worker++;
                    if (bestWrittingArticleCounter < item[i])
                    {
                        ascribeToWorker = worker;
                        bestWrittingArticleCounter = item[i];
                    }
                }

                dic.Add(i, ascribeToWorker);
            }

            foreach (var x in dic)
            {
                stringResult += "Worker " + x.Key.ToString() + " Cooperate " + x.Value.ToString() + Environment.NewLine;
            }

            File.WriteAllText("cooperation.txt", stringResult);
        }
    }
}
