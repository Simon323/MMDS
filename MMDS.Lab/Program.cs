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
            MinHash mi = new MinHash(13);
            Dictionary<int, int[]> exp = new Dictionary<int, int[]>();
            Dictionary<int, HashSet<int>> factsDictionary = CSVProcessing.ReadAllFactsToMemory();

            var watch1 = Stopwatch.StartNew();
            foreach (var x in factsDictionary)
            {
                var y = mi.GenerateHash(x.Value);
                exp.Add(x.Key, y);
            }

            watch1.Stop();
            Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss}", watch1.Elapsed);

            var usersDictionary = factsDictionary.Take(100);
            Dictionary<int, Dictionary<int, double>> result = new Dictionary<int, Dictionary<int, double>>();
            int iterator = 0;
            var watch = Stopwatch.StartNew();

            #region Find 100

                foreach (var user in usersDictionary)
                {
                    Dictionary<int, double> temp = new Dictionary<int, double>();
                    foreach (var fact in factsDictionary)
                    {
                        if (user.Key.Equals(fact.Key))
                            continue;

                        double intersectCount = 0.0;
                        if (user.Value.Count <= fact.Value.Count)
                        {
                            foreach (var y in user.Value)
                            {
                                if (fact.Value.Contains(y))
                                {
                                    intersectCount += 1.0;
                                }
                            }
                        }
                        else
                        {
                            foreach (var y in fact.Value)
                            {
                                if (user.Value.Contains(y))
                                {
                                    intersectCount += 1.0;
                                }
                            }
                        }

                        double unionCounter = (user.Value.Count() + fact.Value.Count()) - Convert.ToInt32(intersectCount);
                        double divideResult = unionCounter.Equals(0) ? 0 : (intersectCount / unionCounter);

                        temp.Add(fact.Key, divideResult);
                    }

                    result.Add(user.Key, temp.OrderByDescending(y => y.Value).Take(100).ToDictionary(y => y.Key, y => y.Value));
                    iterator++;
                }

            #endregion

            watch.Stop();
            Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss}", watch.Elapsed);

            CSVProcessing.ExportToFile(result);

            //Dictionary<int, HashSet<>>
            
            Console.ReadLine();
        }
    }
}
