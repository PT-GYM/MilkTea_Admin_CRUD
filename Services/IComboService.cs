using BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IComboService
    {
        Task<IEnumerable<Combo>> GetAllCombosAsync();
        Task<Combo> GetComboByIdAsync(int id);
        Task AddComboAsync(Combo combo);
        Task UpdateComboAsync(Combo combo);
        Task DeleteComboAsync(int id);
    }
}
