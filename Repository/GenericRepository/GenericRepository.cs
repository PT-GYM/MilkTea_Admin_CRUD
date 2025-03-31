using BussinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _entities;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            if (typeof(T) == typeof(BussinessObject.Combo))
            {
                return (IEnumerable<T>)await _context.Set<BussinessObject.Combo>()
                    .Include(c => c.Products) // EF Core tự ánh xạ nhiều-nhiều
                    .Include(c => c.Toppings) // EF Core tự ánh xạ nhiều-nhiều
                    .ToListAsync();
            }
            return await _entities.ToListAsync();
        }


        public async Task<T?> GetById(int id)
        {
            if (typeof(T) == typeof(BussinessObject.Combo))
            {
                return (T?)(object?)await _context.Set<BussinessObject.Combo>()
                    .Include(c => c.Products)
                    .Include(c => c.Toppings)
                    .FirstOrDefaultAsync(c => c.ComboId == id);
            }
            return await _entities.FindAsync(id);
        }


        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task Update(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> Query() => _entities.AsQueryable();

        public async Task AddProductToComboAsync(int comboId, int productId)
        {
            var combo = await _context.Set<BussinessObject.Combo>()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.ComboId == comboId);

            var product = await _context.Set<BussinessObject.Product>().FindAsync(productId);

            if (combo != null && product != null)
            {
                combo.Products.Add(product);
                await _context.SaveChangesAsync();
            }
        }


        public async Task AddToppingToComboAsync(int comboId, int toppingId)
        {
            var comboTopping = new Dictionary<string, object>
            {
                { "ComboId", comboId },
                { "ToppingId", toppingId }
            };
            await _context.Set<Dictionary<string, object>>("ComboToppings").AddAsync(comboTopping);
        }

        public async Task RemoveProductsFromComboAsync(int comboId)
        {
            var combo = await _context.Set<BussinessObject.Combo>()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.ComboId == comboId);

            if (combo != null)
            {
                combo.Products.Clear();
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveToppingsFromComboAsync(int comboId)
        {
            var comboToppings = _context.Set<Dictionary<string, object>>("ComboToppings")
                .Where(ct => (int)ct["ComboId"] == comboId);
            _context.Set<Dictionary<string, object>>("ComboToppings").RemoveRange(comboToppings);
            await Task.CompletedTask;
        }
    }
}

