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
    public class ToppingService : IToppingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ToppingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Topping>> GetToppingByIdsAsync(List<int> toppingIds)
        {
            var toppings = await _unitOfWork.topping.GetAsync(t => toppingIds.Contains(t.ToppingId));
            return toppings.ToList();  
        }


        public async Task<IEnumerable<Topping>> GetAllToppingsAsync()
        {
            return await _unitOfWork.topping.GetAsync();
        }

        public async Task<Topping> GetToppingByIdAsync(int id)
        {
            return await _unitOfWork.topping.GetByIdAsync(id);
        }

        public async Task AddToppingAsync(Topping topping)
        {
            await _unitOfWork.topping.AddAsync(topping);
            var changes = await _unitOfWork.SaveAsync();
            if (changes == 0)
            {
                throw new Exception("Failed to add topping.");
            }
        }

        public async Task UpdateToppingAsync(Topping topping)
        {
            var existingTopping = await _unitOfWork.topping.GetByIdAsync(topping.ToppingId);
            if (existingTopping == null)
            {
                throw new Exception("Topping not found.");
            }

            existingTopping.Name = topping.Name;
            existingTopping.Price = topping.Price;

            if (!string.IsNullOrEmpty(topping.ImageUrl))
            {
                existingTopping.ImageUrl = topping.ImageUrl;
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteToppingAsync(int id)
        {
            var topping = await _unitOfWork.topping.GetByIdAsync(id);
            if (topping == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            _unitOfWork.topping.Delete(topping);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> CheckAndUpdateToppingStockAsync(List<Topping> toppings)
        {
            foreach (var topping in toppings)
            {
                if (topping.Stock <= 0)
                {
                    return false; 
                }

                topping.Stock -= 1; 
                await UpdateToppingAsync(topping);
            }
            return true;
        }
    }
}
