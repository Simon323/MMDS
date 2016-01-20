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
            Dictionary<int, Dictionary<int, int>> factsDictionary = CSVProcessing.ReadAllFactsToMemory();
            var usersDictionary = factsDictionary.Take(100);
            Dictionary<int, Dictionary<int, double>> result = new Dictionary<int, Dictionary<int, double>>();
            int iterator = 0;
            var watch = Stopwatch.StartNew();

            foreach (var user in usersDictionary)
            {
                Dictionary<int, double> temp = new Dictionary<int, double>();
                foreach (var fact in factsDictionary)
                {
                    if (user.Key.Equals(fact.Key))
                        continue;

                    //double intersectCount = user.Value.Intersect(fact.Value).Count();
                    double intersectCount = 0.0;
                    if(user.Value.Count <= fact.Value.Count)
                    {
                        foreach (var x in user.Value)
                        {
                            if (fact.Value.ContainsKey(x.Value))
                            {
                                intersectCount += 1.0;
                            }
                        }
                    }
                    else
                    {
                        foreach (var x in fact.Value)
                        {
                            if (user.Value.ContainsKey(x.Value))
                            {
                                intersectCount += 1.0;
                            }
                        }
                    }

                    double unionCounter = (user.Value.Count() + fact.Value.Count()) - Convert.ToInt32(intersectCount); 
                    double divideResult = unionCounter.Equals(0) ? 0 : (intersectCount / unionCounter);

                    temp.Add(fact.Key, divideResult);
                }
                
                result.Add(user.Key, temp.OrderByDescending(x => x.Value).Take(100).ToDictionary(x => x.Key, x => x.Value));
                iterator++;
            }
            
            watch.Stop();
            Console.WriteLine("Time elapsed: {0:hh\\:mm\\:ss}", watch.Elapsed);

            CSVProcessing.ExportToFile(result);

            Console.ReadLine();
        }
    }
}
