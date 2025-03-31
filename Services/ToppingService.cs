using BussinessObject;
using Repository.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ToppingService : IToppingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ToppingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Topping>> GetAllToppingsAsync()
        {
            return await _unitOfWork.topping.GetAll();
        }

        public async Task<Topping> GetToppingByIdAsync(int id)
        {
            return await _unitOfWork.topping.GetById(id);
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
            var existingTopping = await _unitOfWork.topping.GetById(topping.ToppingId);
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
            var topping = await _unitOfWork.topping.GetById(id);
            if (topping == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            await _unitOfWork.topping.Delete(topping);
            await _unitOfWork.SaveAsync();
        }
    }
}
