using MMDS.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMDS.Model.Repository.Interfaces
{
    public interface ISearchRepository
    {
        IQueryable<search> GetAll();
        int CountForSingleWord(string word);
        int CountForPairWord(string wordOne, string wordTwo);
    }
}
