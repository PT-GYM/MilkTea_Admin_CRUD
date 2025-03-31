using BussinessObject;
using Repository.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService : ICategory
    {
        private readonly IUnitOfWork _unit;

        public CategoryService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<Category> GetCatgoryByID(int id)
        {
            return await _unit.category.GetById(id);
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _unit.category.GetAll();
        }
    }
}




