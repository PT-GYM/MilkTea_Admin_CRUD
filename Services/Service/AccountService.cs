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
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Login(string username, string password)
        {
            return await _unitOfWork.account.Query().FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
        }
    }
}
