using BussinessObject;
using Microsoft.EntityFrameworkCore;
using Repository.UnitOfWorks;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubService _hubNoti;

        public ProductService(IUnitOfWork unitOfWork, IHubService hubNoti)
        {
            _unitOfWork = unitOfWork;
            _hubNoti = hubNoti;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _unitOfWork.product.Query().Include(p => p.Category).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _unitOfWork.product.GetByIdAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
             _unitOfWork.product.AddAsync(product);
            var changes = await _unitOfWork.SaveAsync();
            await _hubNoti.NotiProCreated(product);
            if (changes == 0)
            {
                throw new Exception("Failed to add product.");
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _unitOfWork.product.GetByIdAsync(product.ProductId);
            if (existingProduct == null)
            {
                throw new Exception("Product not found.");
            }

            // Cập nhật thông tin sản phẩm
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.Stock = product.Stock;

            // Chỉ cập nhật ảnh mới nếu có
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                existingProduct.ImageUrl = product.ImageUrl;
            }

            // Cập nhật trong DbContext
            _unitOfWork.product.Update(existingProduct);

            // Lưu thay đổi
            await _unitOfWork.SaveAsync();
            await _hubNoti.NotiProUpdated(existingProduct);
        }


        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.product.GetByIdAsync(id);
            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            _unitOfWork.product.Delete(product);
            await _unitOfWork.SaveAsync(); 
            await _hubNoti.NotiProDeleted(id);
        }

        public async Task<List<Product>> GetProductByIdsAsync(List<int> productIds)
        {
            var product = await _unitOfWork.product.GetAsync(t => productIds.Contains(t.ProductId));
            return product.ToList();
        }

        public async Task<bool> CheckAndUpdateProductStockAsync(Product product, int quantity = 1)
        {
            if (product.Stock < quantity)
            {
                return false; 
            }

            product.Stock -= quantity;
            await UpdateProductAsync(product); 
            return true;
        }
    }
}



