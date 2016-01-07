using MMDS.Model.Repository;
using MMDS.Model.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;

namespace MMDS.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            IOfferRepository offerRepositiry = new OfferRepository();
            ISearchRepository searchRepository = new SearchRepository();
            List<string> topWordsList = GetTopWordsList();
            //CountForSingleWord(topWordsList, searchRepository);
            //CountForPairWords(topWordsList, searchRepository);
            CompareOffer(topWordsList, offerRepositiry);

        }

        public static List<string> GetTopWordsList()
        {
            List<string> topWords = new List<string>();

            #region Add to list
            
            topWords.Add("samsung");
            topWords.Add("galaxy");
            topWords.Add("etui");
            topWords.Add("sony");
            topWords.Add("iphone");
            topWords.Add("xperia");
            topWords.Add("nokia");
            topWords.Add("lg");
            topWords.Add("usb");
            topWords.Add("bateria");

            topWords.Add("lumia");
            topWords.Add("mini");
            topWords.Add("hp");
            topWords.Add("folia");
            topWords.Add("htc");
            topWords.Add("lenovo");
            topWords.Add("dell");
            topWords.Add("obudowa");
            topWords.Add("pokrowiec");
            topWords.Add("radio");

            topWords.Add("laptop");
            topWords.Add("tablet");
            topWords.Add("s4");
            topWords.Add("lte");
            topWords.Add("uchwyt");
            topWords.Add("lcd");
            topWords.Add("telefon");
            topWords.Add("s5");
            topWords.Add("huawei");
            topWords.Add("pro");

            topWords.Add("kabel");
            topWords.Add("karta");
            topWords.Add("s3");
            topWords.Add("plus");
            topWords.Add("kamera");
            //topWords.Add("@case");
            topWords.Add("led");
            topWords.Add("fv");
            topWords.Add("wifi");
            topWords.Add("nowy");

            topWords.Add("apple");
            topWords.Add("odkurzacz");
            topWords.Add("klawiatura");
            topWords.Add("zasilacz");
            topWords.Add("antena");
            topWords.Add("bluetooth");
            topWords.Add("zestaw");
            topWords.Add("słuchawki");
            topWords.Add("asus");
            topWords.Add("canon");

            topWords.Add("C8gb");
            topWords.Add("C5s");
            topWords.Add("dysk");
            topWords.Add("Ładowarka");
            topWords.Add("xbox");
            topWords.Add("laptopa");
            topWords.Add("C4s");
            topWords.Add("z1");
            topWords.Add("hd");
            topWords.Add("desire");

            topWords.Add("sd");
            topWords.Add("tv");
            topWords.Add("C4gb");
            topWords.Add("adapter");
            topWords.Add("tusz");
            topWords.Add("nowa");
            topWords.Add("pl");
            topWords.Add("note");
            topWords.Add("core");
            topWords.Add("gps");

            topWords.Add("nikon");
            topWords.Add("grand");
            topWords.Add("windows");
            topWords.Add("router");
            topWords.Add("s6");
            topWords.Add("micro");
            topWords.Add("hdmi");
            topWords.Add("nawigacja");
            topWords.Add("philips");
            topWords.Add("mp3");

            topWords.Add("dotyk");
            topWords.Add("prime");
            topWords.Add("monitor");
            topWords.Add("samochodowe");
            topWords.Add("C16gb");
            topWords.Add("pilot");
            topWords.Add("ssd");
            topWords.Add("tab");
            topWords.Add("stacja");
            topWords.Add("futerał");

            topWords.Add("trend");
            topWords.Add("czarny");
            topWords.Add("wzmacniacz");
            topWords.Add("kabura");
            topWords.Add("telewizor");
            topWords.Add("samochodowy");
            topWords.Add("gw");
            topWords.Add("dual");
            topWords.Add("ramka");
            topWords.Add("hartowane_");

            #endregion

            return topWords;

        }

        public static void CountForSingleWord(List<string> topWordsList, ISearchRepository searchRepository)
        {
            List<TopOneWord> resultOneWord = new List<TopOneWord>();
            int progress = 0;

            Console.WriteLine(progress);

            var watch = Stopwatch.StartNew();

            foreach (var word in topWordsList)
            {
                resultOneWord.Add(new TopOneWord(word, searchRepository.CountForSingleWord(word)));
                Console.Clear();
                progress++;
                Console.WriteLine(progress);
            }
            watch.Stop();
            Console.Clear();
            Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss}", watch.Elapsed);

            resultOneWord = resultOneWord.OrderByDescending(x => x.count).ToList();

            
            foreach (var word in resultOneWord)
            {
                Console.WriteLine(word.word + " " + word.count);
            }
        }

        public static void CountForPairWords(List<string> topWordsList, ISearchRepository searchRepository)
        {
            List<TopPairWords> resultPairWords = new List<TopPairWords>();
            int[,] tab = new int[100, 100];
            int progress = 0;
            Console.WriteLine(progress);

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    string wordOne = topWordsList.ElementAt(i);
                    string wordTwo = topWordsList.ElementAt(j);
                    resultPairWords.Add(new TopPairWords(wordOne, wordTwo, searchRepository.CountForPairWord(wordOne, wordTwo)));
                    Console.Clear();
                    progress++;
                    Console.WriteLine(progress);
                }
            }
            watch.Stop();
            Console.Clear();
            Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss}", watch.Elapsed);

            resultPairWords = resultPairWords.OrderByDescending(x => x.count).ToList();

            
            foreach (var pair in resultPairWords)
            {
                Console.WriteLine(pair.wordOne + ", " + pair.wordTwo + " " + pair.count);
            }
        }

        public static void CompareOffer(List<string> topWordsList, IOfferRepository offerRepositiry)
        {
            //List<string> result = one.Union(two).ToList();

            var listAllOffer = offerRepositiry.GetAll().OrderBy(x => x.IT_ID).ToList();
            List<Offer> result = new List<Offer>();
            double progress = 0.0;
            var watch = Stopwatch.StartNew();

            foreach (var offer in listAllOffer.Take(100))
            {
                List<string> keyWordsList = new List<string>();
                foreach (var word in topWordsList)
                {
                    var value = offer.GetType().GetProperty(word).GetValue(offer, null);
                    if (value.Equals("1"))
                    {
                        keyWordsList.Add(word);
                    }
                }

                if(keyWordsList.Count() > 0)
                {
                    result.Add(new Offer(offer.IT_ID, keyWordsList));
                }
            }

            Console.WriteLine(progress);

            foreach (var item in result)
            {
                progress += 1.28;
                string query = String.Empty;

                var last = item.keyWordsList.Last();
                foreach (var attr in item.keyWordsList)
                {
                    if (attr.Equals(last))
                    {
                        query += attr + "=\"1\"";
                    }
                    else
                    {
                        query += attr + "=\"1\" OR ";
                    }
                }

                var listAllSimilarOffer = listAllOffer.Where(query).ToList();
                listAllSimilarOffer.Remove(listAllOffer.FirstOrDefault(x => x.IT_ID.Equals(item.offerId)));

                foreach(var similarOffer in listAllSimilarOffer)
                {
                    List<string> keyWordsList = new List<string>();
                    foreach (var word in topWordsList)
                    {
                        var value = similarOffer.GetType().GetProperty(word).GetValue(similarOffer, null);
                        if (value.Equals("1"))
                        {
                            keyWordsList.Add(word);
                        }
                    }

                    double intersectCount = item.keyWordsList.Select(x => x).Intersect(keyWordsList).Count();
                    double unionCounter = item.keyWordsList.Union(keyWordsList).ToList().Count();
                    double divideResult = unionCounter.Equals(0) ? 0 : (intersectCount / unionCounter);
                    item.similarOfferList.Add(new Similar(similarOffer.IT_ID, divideResult));
                }

                Console.Clear();
                progress += 1.28;
                Console.Write(progress);
            }

            watch.Stop();
            Console.Clear();
            Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss}", watch.Elapsed);

            foreach (var item in result)
            {
                item.similarOfferList = item.similarOfferList.OrderByDescending(x => x.percentSimilar).ToList();

                Console.WriteLine("Offer: " + item.offerId);
                Console.WriteLine("Similar: " + item.similarOfferList.First().offerId + " Percent: " + item.similarOfferList.First().percentSimilar);
                Console.WriteLine();
            }
        }

         
    }
}
