using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RentCar.Models;
using RentCar.Models.ViewModels;
using RentCar.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace RentCar.Controllers
{
    [Route("Aluguel")]
    [Authorize]
    public class RentsController : Controller
    {
        private readonly IRentService _rentService;
        private readonly ICarService _carService;
        private readonly IMemoryCache _cache;
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;

        // Tempo de duração do Cache
        private readonly MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
        // Lista para guardar cache
        private List<Rent> list;

        public RentsController(IRentService rentService, ICarService carService, IMemoryCache cache, IAccountService accountService, UserManager<User> userManager)
        {
            _rentService = rentService;
            _carService = carService;
            _cache = cache;
            _accountService = accountService;
            _userManager = userManager;
        }



        // Criar Get:
        [HttpGet("Novo")]
        public async Task<IActionResult> Create(int id)
        {
            var car = await _carService.FindByIdAsync(id);
            var rent = new Rent { CarId = id };
            var viewModel = new RentFormViewModel { Car = car, Rent = rent };
            return View(viewModel);
        }

        // Criar Post: 
        [HttpPost("Novo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentFormViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var account = await _accountService.GetAccountByUser(user);
                var car = await _carService.FindByIdAsync(obj.Rent.CarId);
                var value = car.DailyPrice * obj.Rent.End.Subtract(obj.Rent.Start).Days;
                if (account.Balance < value)
                {
                    TempData["erro"] = "Saldo insuficiente para alugar o veículo.";
                    return RedirectToAction("Index", "Cars");
                }

                account.Balance -= obj.Rent.Value;
                await _accountService.UpdateAsync(account);

                TempData["confirm"] = obj.Car.Brand + " - " + obj.Car.Model + " foi alugado com sucesso.";
                obj.Rent.UserId = user.Id;
                await _rentService.InsertAsync(obj.Rent);
                list = await _rentService.FindAllAsync();
                _cache.Set("rent", list, cacheOptions);

                return RedirectToAction("Index", "Accounts");
            }

            TempData["erro"] = "Erro ao alugar.";
            return RedirectToAction("Index", "Cars");
        }

        // Verificar se saldo é suficiente:
        [HttpGet("Saldo-suficiente")]
        public async Task<JsonResult> EnoughBalance([Bind(Prefix ="Rent")] Rent obj)
        {
            var user = await _userManager.GetUserAsync(User);
            var account = await _accountService.GetAccountByUser(user);
            if (account == null)
            {
                return Json("não há saldo suficiente (Saldo atual: R$0,00)");
            }
            else if (obj.Value / 100.00 > account.Balance)
            {
                return Json("não há saldo suficiente (Saldo atual: " + account.Balance.ToString("C2") + ")");
            }
            return Json(true);
        }
    }
}