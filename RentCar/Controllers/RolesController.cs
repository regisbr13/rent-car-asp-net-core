using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RentCar.Data;
using RentCar.Models;
using RentCar.Models.ViewModels;
using RentCar.Services.Exceptions;

namespace RentCar.Controllers
{
    [Route("Niveis-de-acesso")]
    public class RolesController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<RolesController> _logger;
        private readonly IMemoryCache _cache;

        // Tempo de duração do Cache
        private readonly MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
        // Lista para guardar cache
        private List<Role> list;

        public RolesController(RoleManager<Role> roleManager, ILogger<RolesController> logger, IMemoryCache cache)
        {
            _roleManager = roleManager;
            _logger = logger;
            _cache = cache;
        }


        // Listar Get:
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            if (!_cache.TryGetValue("role", out list))
            {
                list = await _roleManager.Roles.ToListAsync();
                _cache.Set("role", list, cacheOptions);
            }
            else
            {
                list = _cache.Get("role") as List<Role>;
            }
            _logger.LogInformation("Listando todos níveis de acesso");
            return View(list.OrderBy(x => x.Name));
        }

        // Criar Get:
        [HttpGet("Novo")]
        public IActionResult Create()
        {
            _logger.LogInformation("Criando nível de acesso");
            return View();
        }

        // Criar Post: 
        [HttpPost("Novo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role obj)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Verificando se o nível de acesso já existe");
                if(!await _roleManager.RoleExistsAsync(obj.Name))
                {
                    obj.NormalizedName = obj.Name.ToUpper();
                    await _roleManager.CreateAsync(obj);
                    TempData["confirm"] = obj.Name + " foi cadastrado com sucesso.";
                    _logger.LogInformation("Nível de acesso criado");
                    list = await _roleManager.Roles.ToListAsync();
                    _cache.Set("role", list, cacheOptions);
                }
                else
                {
                    TempData["erro"] = obj.Name + " já está registrado";
                    _logger.LogInformation("Nível de acesso já existe");
                }

                return RedirectToAction(nameof(Index));
            }

            TempData["erro"] = "Erro ao cadastrar.";
            _logger.LogInformation("Nível de acesso não pode ser cadastrado");

            return RedirectToAction(nameof(Index));
        }

        // Editar Get:
        [HttpGet("Editar")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                _logger.LogInformation("Id é nulo");

                return RedirectToAction(nameof(Error), new { message = "Id nulo" });
            }

            if (!_cache.TryGetValue("role", out list))
            {
                list = await _roleManager.Roles.ToListAsync();
                _cache.Set("role", list, cacheOptions);
            }
            else
            {
                list = _cache.Get("role") as List<Role>;
            }

            var obj = list.Find(x => x.Id == id);
            if (obj == null)
            {
                _logger.LogInformation("Nível o nível de acesso não existe");

                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            }

            var viewModel = new RoleFormViewModel { Description = obj.Description, Name = obj.Name, Id = obj.Id, NormalizedName = obj.NormalizedName, ConcurrencyStamp = obj.ConcurrencyStamp };

            return View(viewModel);
        }

        // Editar Post:
        [HttpPost("Editar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Role obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _roleManager.UpdateAsync(obj);
                    if (!result.Succeeded)
                    {
                        TempData["erro"] = result.Errors.First().Description.ToString();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["confirm"] = obj.Name + " foi atualizado com sucesso.";
                    }
                    list = await _roleManager.Roles.ToListAsync();
                    _cache.Set("role", list, cacheOptions);

                    _logger.LogInformation("Nível de acesso atualizado");

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

        // Delete Get:
        [HttpGet("Excluir")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id nulo" });
            }

            if (!_cache.TryGetValue("role", out list))
            {
                list = await _roleManager.Roles.ToListAsync();
                _cache.Set("role", list, cacheOptions);
            }
            else
            {
                list = _cache.Get("role") as List<Role>;
            }

            var obj = list.Find(x => x.Id == id);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            }

            return View(obj);
        }

        // Delete Post: 
        [HttpPost("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Role obj)
        {
            try
            {
                var result = await _roleManager.DeleteAsync(obj);
                if (!result.Succeeded)
                {
                    TempData["erro"] = result.Errors.First().ToString();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["confirm"] = obj.Name + " foi deletado com sucesso.";
                }
                list = await _roleManager.Roles.ToListAsync();
                _cache.Set("role", list, cacheOptions);
                _logger.LogInformation("Nível de acesso exluído");

                return RedirectToAction(nameof(Index));
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

        [HttpGet("Existe")]
        public async Task<JsonResult> RoleExist(string Name)
        {
            if(await _roleManager.RoleExistsAsync(Name))
            {
               return Json("nível de acesso já cadastrado");
            }
            return Json(true);
        }
    }
}
