using BussinessObject;
using Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MilkTeaShopContext _context;
        public IGenericRepository<User> account {  get; }

        public IGenericRepository<Product> product {  get; }

        public IGenericRepository<Topping> topping {  get; }

        public IGenericRepository<Combo> combo { get; }
        public IGenericRepository<Category> category { get; }

        public UnitOfWork(MilkTeaShopContext context)
        {
            _context = context;
            account = new GenericRepository<User>(_context);
            product = new GenericRepository<Product>(_context);
            combo = new GenericRepository<Combo>(_context);
            topping = new GenericRepository<Topping>(_context);
            category = new GenericRepository<Category>(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}


