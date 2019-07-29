using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentCar.Models;
using RentCar.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace RentCar.Controllers
{
    [Route("Saldos")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;
        private readonly IMemoryCache _cache;

        // Tempo de duração do Cache
        private readonly MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
        // Lista para guardar cache
        private List<Account> list;

        public AccountController(IAccountService accountService, UserManager<User> userManager, IMemoryCache cache)
        {
            _accountService = accountService;
            _userManager = userManager;
            _cache = cache;
        }

        // Listar Get:
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            if (!_cache.TryGetValue("account", out list))
            {
                list = await _accountService.FindAllAsync();
                _cache.Set("account", list, cacheOptions);
            }
            else
            {
                list = _cache.Get("account") as List<Account>;
            }
            return View(list);
        }

        // Criar Get:
        [HttpGet("Novo")]
        public async Task<IActionResult> Create()
        {
            var list = await _userManager.Users.ToListAsync();
            ViewBag.users = new SelectList(list, "Id", "Name");
            return View();
        }

        // Criar Post: 
        [HttpPost("Novo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account obj)
        {
            if (ModelState.IsValid)
            {
                TempData["confirm"] = "O saldo foi cadastrado com sucesso.";
                await _accountService.InsertAsync(obj);
                list = await _accountService.FindAllAsync();
                _cache.Set("account", list, cacheOptions);
                return RedirectToAction(nameof(Index));
            }

            TempData["erro"] = "Erro ao cadastrar.";
            return RedirectToAction(nameof(Index));
        }

        // Editar Get:
        [HttpGet("Editar")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id nulo" });
            }

            if (!_cache.TryGetValue("account", out list))
            {
                list = await _accountService.FindAllAsync();
                _cache.Set("account", list, cacheOptions);
            }
            else
            {
                list = _cache.Get("account") as List<Account>;
            }

            var obj = list.Find(x => x.Id == id);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            }

            return View(obj);
        }

        // Editar Post:
        [HttpPost("Editar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Account obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _accountService.UpdateAsync(obj);
                    TempData["confirm"] = "O saldo foi editado com sucesso.";
                    var account = (_cache.Get("account") as List<Account>).Find(x => x.Id == obj.Id);
                    list = await _accountService.FindAllAsync();
                    _cache.Set("account", list, cacheOptions);

                    return RedirectToAction(nameof(Index));
                }
                catch (ApplicationException e)
                {
                    return RedirectToAction(nameof(Error), new { message = e.Message });
                }
            }

            if (id != obj.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id's não correspondem" });
            }

            TempData["erro"] = "Erro ao editar.";
            return RedirectToAction(nameof(Index));
        }

        // Tratamento de erros:
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}