﻿using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IToppingService
    {
        Task<IEnumerable<Topping>> GetAllToppingsAsync();
        Task<Topping> GetToppingByIdAsync(int id);
        Task AddToppingAsync(Topping topping);
        Task UpdateToppingAsync(Topping topping);
        Task DeleteToppingAsync(int id);
    }
}
