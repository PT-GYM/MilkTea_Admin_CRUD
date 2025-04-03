using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IComboService
    {
        Task<IEnumerable<Combo>> GetAllCombos();
        Task<Combo> GetComboById(int comboId);
        Task<bool> DeleteCombo(int comboId);
        Task<bool> AddCombo(Combo combo);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Topping>> GetAllToppingsAsync();
    }
}
