using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
    using System.Threading.Tasks;

    namespace Repository.GenericRepository
    {
        public interface IGenericRepository<T> where T : class
        {
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Query();
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "");
    }
    }