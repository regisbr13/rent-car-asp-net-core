using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RentCar.Models;
using RentCar.Services.Exceptions;
using RentCar.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace RentCar.Controllers
{
    [Route("Carros")]
    [Authorize]
    public class CarsController : Controller
    {
        private readonly ICarService _carService;
        private readonly IMemoryCache _cache;
        private readonly IHostingEnvironment _environment;

        // Tempo de duração do Cache
        private readonly MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60));
        // Lista para guardar cache
        private List<Car> list;

        public CarsController(ICarService carService, IMemoryCache cache, IHostingEnvironment environment)
        {
            _carService = carService;
            _cache = cache;
            _environment = environment;
        }


        // Listar Get:
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            if (!_cache.TryGetValue("car", out list))
            {
                list = await _carService.FindAllAsync();
                _cache.Set("car", list, cacheOptions);
            }
            else
            {
                list = _cache.Get("car") as List<Car>;
            }
            return View(list);
        }

        // Criar Get:
        [HttpGet("Novo")]
        public IActionResult Create()
        {
            return View();
        }

        // Criar Post: 
        [HttpPost("Novo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car obj, IFormFile img)
        {
            if (ModelState.IsValid)
            {
                if(img != null)
                {
                    var path = Path.Combine(_environment.WebRootPath, "images");
                    using (FileStream fs = new FileStream(Path.Combine(path, img.FileName), FileMode.Create))
                    {
                        await img.CopyToAsync(fs);
                        obj.ImgPath = "~/images/" + img.FileName;
                    }
                }
                else
                {
                    obj.ImgPath = "~/images/car.png";
                }
                TempData["confirm"] = obj.Model + " foi cadastrado com sucesso.";
                await _carService.InsertAsync(obj);
                list = await _carService.FindAllAsync();
                _cache.Set("car", list, cacheOptions);
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

            if (!_cache.TryGetValue("car", out list))
            {
                list = await _carService.FindAllAsync();
                _cache.Set("car", list, cacheOptions);
            }
            else
            {
                list = _cache.Get("car") as List<Car>;
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
        public async Task<IActionResult> Edit(int id, Car obj, IFormFile img)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (img != null && "~/images/" + img.FileName != obj.ImgPath)
                    {
                        var path = Path.Combine(_environment.WebRootPath, "images");
                        using (FileStream fs = new FileStream(Path.Combine(path, img.FileName), FileMode.Create))
                        {
                            await img.CopyToAsync(fs);
                            obj.ImgPath = "~/images/" + img.FileName;
                        }
                    }

                    TempData["confirm"] = obj.Model + " foi atualizado com sucesso.";
                    await _carService.UpdateAsync(obj);
                    list = await _carService.FindAllAsync();
                    _cache.Set("car", list, cacheOptions);
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
        [HttpGet("Deletar")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id nulo" });
            }

            if (!_cache.TryGetValue("car", out list))
            {
                list = await _carService.FindAllAsync();
                _cache.Set("car", list, cacheOptions);
            }
            else
            {
                list = _cache.Get("car") as List<Car>;
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
                await _carService.RemoveAsync(id);
                var obj = (_cache.Get("car") as List<Car>).Find(x => x.Id == id);
                TempData["confirm"] = obj.Model + " foi deletado com sucesso.";
                string path = obj.ImgPath;
                path = path.Replace("~", "wwwroot");
                System.IO.File.Delete(path);

                (_cache.Get("car") as List<Car>).Remove(obj);
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
    }
}