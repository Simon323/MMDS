using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMDS.Lab
{
    public class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, List<int>> factsDictionary = CSVProcessing.ReadAllFactsToMemory();
            var usersDictionary = factsDictionary.Take(100);
            Dictionary<int, List<string>> result = new Dictionary<int, List<string>>();
            int iterator = 0;
            var watch = Stopwatch.StartNew();

            foreach (var user in usersDictionary)
            {
                Dictionary<int, double> temp = new Dictionary<int, double>();
                foreach (var fact in factsDictionary)
                {
                    if (user.Key.Equals(fact.Key))
                        continue;

                    double intersectCount = user.Value.Select(x => x).Intersect(fact.Value).Count();
                    double unionCounter = user.Value.Union(fact.Value).ToList().Count();
                    double divideResult = unionCounter.Equals(0) ? 0 : (intersectCount / unionCounter);

                    temp.Add(fact.Key, divideResult);
                }

                List<string> res = new List<string>();
                foreach (var x in temp.OrderByDescending(x => x.Value).Take(100))
                {
                    res.Add(x.Key + " " + x.Value);
                }

                result.Add(user.Key, res);
                iterator++;
            }

            watch.Stop();
            Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss}", watch.Elapsed);

            Console.ReadLine();
        }
    }
}
