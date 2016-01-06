using MMDS.DBContext.Repository;
using MMDS.Model.Model;
using MMDS.Model.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMDS.Model.Repository
{
    public class SearchRepository : BaseDbContextRepository<search, AllegroEntities>, ISearchRepository
    {
    }
}
