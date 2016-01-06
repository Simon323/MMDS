﻿using MMDS.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMDS.Model.Repository.Interfaces
{
    public interface IOfferRepository
    {
        IQueryable<offer> GetAll();
    }
}
