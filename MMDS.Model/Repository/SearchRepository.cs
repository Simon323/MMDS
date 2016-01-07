﻿using MMDS.DBContext.Repository;
using MMDS.Model.Model;
using MMDS.Model.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;

namespace MMDS.Model.Repository
{
    public class SearchRepository : BaseDbContextRepository<search, AllegroEntities>, ISearchRepository
    {
        public int CountForSingleWord(string word)
        {
            //return Items.Where(x => x.samsung.Equals("1")).Count();
            return Items.Where(word + "=\"1\"").Count();
        }

        public int CountForPairWord(string wordOne, string wordTwo)
        {
            //return Items.Where(x => x.samsung.Equals("1")).Count();
            return Items.Where(wordOne + "=\"1\" AND " + wordTwo + "=\"1\"").Count();
        }
    }
}
