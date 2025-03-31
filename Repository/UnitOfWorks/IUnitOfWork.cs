using BussinessObject;
using Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IGenericRepository<User> account { get; }
        IGenericRepository<Product> product { get; }
        IGenericRepository<Topping> topping { get; }
        IGenericRepository<Combo> combo { get; }
        IGenericRepository<Category> category { get; }
        IGenericRepository<Order> order { get; }
        IGenericRepository<OrderDetail> orderDetail { get; }
        Task<int> SaveAsync();
    }
}

