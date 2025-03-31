using BussinessObject;
using Repository.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ComboService : IComboService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ComboService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Combo>> GetAllCombosAsync()
        {
            return await _unitOfWork.combo.GetAll();
        }

        public async Task<Combo> GetComboByIdAsync(int id)
        {
            return await _unitOfWork.combo.GetById(id);
        }

        public async Task AddComboAsync(Combo combo)
        {
            // Kiểm tra null
            combo.Products = combo.Products ?? new List<Product>();
            combo.Toppings = combo.Toppings ?? new List<Topping>();

            // Kiểm tra giới hạn
            if (combo.Products.Count > 3 || combo.Toppings.Count > 3)
            {
                throw new ArgumentException("Combo can only have up to 3 products and 3 toppings.");
            }

            // Loại bỏ trùng lặp
            combo.Products = combo.Products.DistinctBy(p => p.ProductId).ToList();
            combo.Toppings = combo.Toppings.DistinctBy(t => t.ToppingId).ToList();

            // Thêm combo
            await _unitOfWork.combo.AddAsync(combo);
            var changes = await _unitOfWork.SaveAsync();
            if (changes == 0)
            {
                throw new Exception("Failed to add combo.");
            }

            // Liên kết sản phẩm và topping với combo
            var productList = combo.Products.ToList();  // Tạo bản sao
            foreach (var product in productList)
            {
                var existingProduct = await _unitOfWork.product.GetById(product.ProductId);
                if (existingProduct != null)
                {
                    await _unitOfWork.combo.AddProductToComboAsync(combo.ComboId, product.ProductId);
                }
            }

            var toppingList = combo.Toppings.ToList();  // Tạo bản sao
            foreach (var topping in toppingList)
            {
                var existingTopping = await _unitOfWork.topping.GetById(topping.ToppingId);
                if (existingTopping != null)
                {
                    await _unitOfWork.combo.AddToppingToComboAsync(combo.ComboId, topping.ToppingId);
                }
            }

            changes = await _unitOfWork.SaveAsync();
            if (changes == 0 && (combo.Products.Count > 0 || combo.Toppings.Count > 0))
            {
                throw new Exception("Failed to add products or toppings to combo.");
            }
        }

        public async Task UpdateComboAsync(Combo combo)
        {
            // Kiểm tra null
            combo.Products = combo.Products ?? new List<Product>();
            combo.Toppings = combo.Toppings ?? new List<Topping>();

            // Kiểm tra giới hạn
            if (combo.Products.Count > 3 || combo.Toppings.Count > 3)
            {
                throw new ArgumentException("Combo can only have up to 3 products and 3 toppings.");
            }

            // Loại bỏ trùng lặp
            combo.Products = combo.Products.DistinctBy(p => p.ProductId).ToList();
            combo.Toppings = combo.Toppings.DistinctBy(t => t.ToppingId).ToList();

            // Cập nhật combo
            await _unitOfWork.combo.Update(combo);

            // Xóa các liên kết cũ
            await _unitOfWork.combo.RemoveProductsFromComboAsync(combo.ComboId);
            await _unitOfWork.combo.RemoveToppingsFromComboAsync(combo.ComboId);

            // Thêm lại liên kết mới
            var productList = combo.Products.ToList();  // Tạo bản sao
            foreach (var product in productList)
            {
                var existingProduct = await _unitOfWork.product.GetById(product.ProductId);
                if (existingProduct != null)
                {
                    await _unitOfWork.combo.AddProductToComboAsync(combo.ComboId, product.ProductId);
                }
            }

            var toppingList = combo.Toppings.ToList();  // Tạo bản sao
            foreach (var topping in toppingList)
            {
                var existingTopping = await _unitOfWork.topping.GetById(topping.ToppingId);
                if (existingTopping != null)
                {
                    await _unitOfWork.combo.AddToppingToComboAsync(combo.ComboId, topping.ToppingId);
                }
            }

            var changes = await _unitOfWork.SaveAsync();
            if (changes == 0)
            {
                throw new Exception("Failed to update combo.");
            }
        }

        public async Task DeleteComboAsync(int id)
        {
            var combo = await _unitOfWork.combo.GetById(id);
            if (combo != null)
            {
                await _unitOfWork.combo.Delete(combo);
                var changes = await _unitOfWork.SaveAsync();
                if (changes == 0)
                {
                    throw new Exception("Failed to delete combo.");
                }
            }
        }
    }
}
