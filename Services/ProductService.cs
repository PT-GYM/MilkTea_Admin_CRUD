using BussinessObject;
using Microsoft.EntityFrameworkCore;
using Repository.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _unitOfWork.product.Query().Include(p => p.Category).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _unitOfWork.product.GetById(id);
        }

        public async Task AddProductAsync(Product product)
        {
            await _unitOfWork.product.AddAsync(product);
            var changes = await _unitOfWork.SaveAsync();
            if (changes == 0)
            {
                throw new Exception("Failed to add product.");
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _unitOfWork.product.GetById(product.ProductId);
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
            await _unitOfWork.product.Update(existingProduct);

            // Lưu thay đổi
            await _unitOfWork.SaveAsync();
        }


        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.product.GetById(id);
            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            await _unitOfWork.product.Delete(product);
            await _unitOfWork.SaveAsync(); // Không cần kiểm tra changes
        }

    }
}



