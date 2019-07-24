using Microsoft.EntityFrameworkCore;
using RentCar.Data;
using RentCar.Models;
using RentCar.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentCar.Services
{
    public class AccountService : Service<Account>, IAccountService
    {
        private readonly Context _context;
        public AccountService(Context context) : base(context)
        {
            _context = context;
        }

        public new async Task<List<Account>> FindAllAsync()
        {
            return await _context.Accounts.Include(x => x.User).ToListAsync();
        }
    }
}
