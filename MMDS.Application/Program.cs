using MMDS.Model.Repository;
using MMDS.Model.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMDS.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            IOfferRepository xxx = new OfferRepository();
            ISearchRepository searchRepository = new SearchRepository();

            //var searchList = searchRepository.GetAll().ToList();
            //int count = searchList.Count;
            //int i = 0;

            //foreach (var element in searchList)
            //{
            //    i++;
            //    double percent = (i * 100) / count;
            //    Console.Clear();
            //    Console.WriteLine(percent);
            //}

            Console.WriteLine(searchRepository.count());
        }
    }
}
