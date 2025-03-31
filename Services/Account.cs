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
    public class Account : IAccount
    {
        private readonly IUnitOfWork _unitOfWork;

        public Account(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Login(string username, string password)
        {
            return await _unitOfWork.account.Query().FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
        }
    }
}
