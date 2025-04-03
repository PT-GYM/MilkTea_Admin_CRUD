using BussinessObject;
using Microsoft.EntityFrameworkCore;
using Repository.UnitOfWorks;
using Services.Interface;
using System.Threading.Tasks;

namespace Services.Service
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Login(string username, string password)
        {
            return await _unitOfWork.account.Query()
                .FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
        }

        public async Task<bool> Register(User user)
        {
           
            var existingUser = await _unitOfWork.account.Query()
                .AnyAsync(x => x.Username == user.Username);

            if (existingUser)
            {
                return false;
            }

            await _unitOfWork.account.AddAsync(user);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
