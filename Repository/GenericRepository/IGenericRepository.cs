using BussinessObject;
using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Repository.GenericRepository
    {
        public interface IGenericRepository<T> where T : class
        {
            Task<IEnumerable<T>> GetAll();
            Task<T?> GetById(int id); // Sửa từ string sang int
            Task AddAsync(T entity);
            Task Update(T entity);
            Task Delete(T entity);
            IQueryable<T> Query();

            // Phương thức cho mối quan hệ nhiều-nhiều
            Task AddProductToComboAsync(int comboId, int productId);
            Task AddToppingToComboAsync(int comboId, int toppingId);
            Task RemoveProductsFromComboAsync(int comboId);
            Task RemoveToppingsFromComboAsync(int comboId);
        }
    }