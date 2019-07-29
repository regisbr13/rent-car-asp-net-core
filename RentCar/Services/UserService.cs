using Microsoft.EntityFrameworkCore;
using RentCar.Data;
using RentCar.Models;
using RentCar.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentCar.Services
{
    public class UserService : Service<User>, IUserService
    {
        public Context _context { get; set; }
        public UserService(Context context) : base(context)
        {
            _context = context;
        }

        new public async Task<User> FindByIdAsync(string userId)
        {
            return await _context.User.Include(x => x.Addresses).Include(x => x.Rents).ThenInclude(x => x.Car).Include(x => x.Account).FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<bool> CpfExist(string cpf, string id)
        {
            return await _context.User.AnyAsync(x => x.Cpf == cpf && x.Id != id);
        }
    }
}
