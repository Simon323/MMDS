using MMDS.DBContext.Repository;
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
        public int count()
        {
            //return Items.Where(x => x.samsung.Equals("1")).Count();
            return Items.Where("samsung=\"1\"").Count();
        }
    }
}
