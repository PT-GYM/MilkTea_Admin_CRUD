using BussinessObject;
using Repository.UnitOfWorks;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unit;

        public CategoryService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Category> GetCatgoryByID(int id)
        {
            return await _unit.category.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _unit.category.GetAsync();
        }
    }
}




