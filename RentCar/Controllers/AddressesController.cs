using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RentCar.Models;
using RentCar.Services;
using RentCar.Services.Exceptions;
using RentCar.Services.Interfaces;

namespace RentCar.Controllers
{
    [Route("Enderecos")]
    public class AddressesController : Controller
    {
        private readonly IAddressService _adressService;
        private readonly IMemoryCache _cache;

        // Tempo de duração do Cache
        private readonly MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
        // Lista para guardar cache
        private List<Address> list;

        public AddressesController(IAddressService adressService, IMemoryCache cache)
        {
            _adressService = adressService;
            _cache = cache;
        }

        // Detalhar Get:
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id nulo" });
            }

            if (!_cache.TryGetValue("adress", out list))
            {
                list = await _adressService.FindAllAsync();
                _cache.Set("adress", list, cacheOptions);
            }
            else
            {
                list = _cache.Get("adress") as List<Address>;
            }

            var obj = list.Find(x => x.Id == id);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            }

            return View(obj);
        }

        // Criar Get:
        [HttpGet("Novo")]
        public IActionResult Create(string UserId)
        {
            var obj = new Address { UserId = UserId };
            return View(obj);
        }

        // Criar Post: 
        [HttpPost("Novo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Address obj)
        {
            if (ModelState.IsValid)
            {
                TempData["confirm"] = "Endereço cadastrado com sucesso.";
                await _adressService.InsertAsync(obj);
                list = await _adressService.FindAllAsync();
                _cache.Set("adress", list, cacheOptions);
                return RedirectToAction("Index", "Accounts");
            }

            TempData["erro"] = "Erro ao cadastrar.";
            return RedirectToAction("Index", "Accounts");
        }

        // Editar Get:
        [HttpGet("Editar")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id nulo" });
            }

            if (!_cache.TryGetValue("adress", out list))
            {
                list = await _adressService.FindAllAsync();
                _cache.Set("adress", list, cacheOptions);
            }
            else
            {
                list = _cache.Get("adress") as List<Address>;
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
        public async Task<IActionResult> Edit(int id, Address obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _adressService.UpdateAsync(obj);
                    TempData["confirm"] = "Endereço editado com sucesso.";
                    var adress = (_cache.Get("adress") as List<Address>).Find(x => x.Id == obj.Id);
                    (_cache.Get("adress") as List<Address>).Remove(obj);
                    (_cache.Get("adress") as List<Address>).Add(obj);

                    return RedirectToAction("Index", "Accounts");
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
            return RedirectToAction("Index", "Accounts");
        }

        // Delete Get:
        [HttpGet("Deletar")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id nulo" });
            }

            if (!_cache.TryGetValue("adress", out list))
            {
                list = await _adressService.FindAllAsync();
                _cache.Set("adress", list, cacheOptions);
            }
            else
            {
                list = _cache.Get("adress") as List<Address>;
            }

            var obj = list.Find(x => x.Id == id);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            }

            return View(obj);
        }

        // Delete Post: 
        [HttpPost("Deletar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _adressService.RemoveAsync(id);
                var obj = (_cache.Get("adress") as List<Address>).Find(x => x.Id == id);
                TempData["confirm"] = "Endereço deletado com sucesso.";
                (_cache.Get("adress") as List<Address>).Remove(obj);
                return RedirectToAction("Index", "Accounts");
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
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