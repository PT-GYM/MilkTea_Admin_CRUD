﻿using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ICategoryService
    {
        Task<Category> GetCatgoryByID(int id);
        Task<IEnumerable<Category>> GetCategories();

    }
}


