using RentCar.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentCar.Services.Interfaces
{
    public interface IUserService : Iservice<User>
    {
        new Task<User> FindByIdAsync(string userId);
    }
}
