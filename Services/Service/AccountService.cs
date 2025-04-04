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

        public Task<User> Login(string username, string password, string confirmPassword)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Register(string username, string password)
        {
            var exists = await _unitOfWork.account.Query()
                .AnyAsync(u => u.Username == username);

            if (exists)
            {
                return false;
            }

            var newUser = new User
            {
                Username = username,
                Password = password,
                Role = "Customer"
            };

            await _unitOfWork.account.AddAsync(newUser);
            await _unitOfWork.SaveAsync();
            return true;
        }


    }
}
