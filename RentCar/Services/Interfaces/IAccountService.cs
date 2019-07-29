using RentCar.Models;
using System.Threading.Tasks;

namespace RentCar.Services.Interfaces
{
    public interface IAccountService : Iservice<Account>
    {
        Task<Account> GetAccountByUser(User user);
    }
}
