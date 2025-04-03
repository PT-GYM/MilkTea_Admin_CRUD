using BussinessObject;
using Microsoft.EntityFrameworkCore;
using Repository.UnitOfWorks;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Service
{
    public class ComboService : IComboService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        private readonly IToppingService _toppingService;

        public ComboService(IUnitOfWork unitOfWork, IProductService productService, IToppingService toppingService)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
            _toppingService = toppingService;
        }

        public async Task<IEnumerable<Combo>> GetAllCombos()
        {
            var combos = await _unitOfWork.combo.Query().ToListAsync();
            var productNames = await _unitOfWork.product.Query().ToListAsync();
            var toppingNames = await _unitOfWork.topping.Query().ToListAsync();

            foreach (var combo in combos)
            {
                var productIds = combo.ProductIds?.Split(',') ?? new string[0];
                combo.ProductNames = productIds.Select(id => productNames.FirstOrDefault(p => p.ProductId.ToString() == id)?.Name).ToList();

                var toppingIds = combo.ToppingIds?.Split(',') ?? new string[0];
                combo.ToppingNames = toppingIds.Select(id => toppingNames.FirstOrDefault(t => t.ToppingId.ToString() == id)?.Name).ToList();
            }

            return combos;
        }

        public async Task<Combo> GetComboById(int comboId)
        {
            var combo = await _unitOfWork.combo.Query().FirstOrDefaultAsync(c => c.ComboId == comboId);
            if (combo == null) return null;

            var productNames = await _unitOfWork.product.Query().ToListAsync();
            var toppingNames = await _unitOfWork.topping.Query().ToListAsync();

            var productIds = combo.ProductIds?.Split(',') ?? new string[0];
            combo.ProductNames = productIds.Select(id => productNames.FirstOrDefault(p => p.ProductId.ToString() == id)?.Name).ToList();

            var toppingIds = combo.ToppingIds?.Split(',') ?? new string[0];
            combo.ToppingNames = toppingIds.Select(id => toppingNames.FirstOrDefault(t => t.ToppingId.ToString() == id)?.Name).ToList();

            return combo;
        }

        public async Task<bool> AddCombo(Combo combo)
        {
            if (combo == null) return false;

            try
            {
                await _unitOfWork.combo.AddAsync(combo);
                await _unitOfWork.SaveAsync();  
                return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }

        public async Task<bool> DeleteCombo(int comboId)
        {
            var combo = await _unitOfWork.combo.GetByIdAsync(comboId);
            if (combo == null) return false;

            _unitOfWork.combo.Delete(combo);
            await _unitOfWork.SaveAsync();  
            return true;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productService.GetAllProductsAsync();
        }

        public async Task<IEnumerable<Topping>> GetAllToppingsAsync()
        {
            return await _toppingService.GetAllToppingsAsync();
        }
    }
}
