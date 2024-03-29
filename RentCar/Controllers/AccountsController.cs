﻿using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentCar.Models;
using RentCar.Models.ViewModels;
using RentCar.Services.Interfaces;

namespace RentCar.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SignInManager<User> _loginManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly RoleManager<Role> _roleManager;

        public AccountsController(SignInManager<User> loginManager, UserManager<User> userManager, IUserService userService, RoleManager<Role> roleManager)
        {
            _loginManager = loginManager;
            _userManager = userManager;
            _userService = userService;
            _roleManager = roleManager;
        }

        [Authorize]
        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            var user = await _userService.FindByIdAsync(_userManager.GetUserId(User));
            return View(user);
        }

        // Registro de usuários:
        [HttpGet("Registrar")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                _loginManager.SignOutAsync();
            return View();
        }

        [HttpPost("Registrar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterFormViewModel register, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = register.UserName,
                    Email = register.Email,
                    Cpf = register.Cpf,
                    PhoneNumber = register.PhoneNumber,
                    Name = register.Name
                };
                var result = await _userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                    var role = "Cliente";
                    await _userManager.AddToRoleAsync(user, role);

                    await _loginManager.SignInAsync(user, false);
                   return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description.ToString());
                }
            }
            return View(register);
        }

        // Entrada de usuários:
        [HttpGet("Entrar")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Entrar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(int? project, LoginViewModel viewModel, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = new Microsoft.AspNetCore.Identity.SignInResult();
                if (await _userManager.FindByEmailAsync(viewModel.UserName) != null)
                {
                    var user = await _userManager.FindByEmailAsync(viewModel.UserName);
                    result = await _loginManager.PasswordSignInAsync(user.UserName, viewModel.Password, viewModel.Checkbox, lockoutOnFailure: false);
                }
                else
                {
                    var user = await _userManager.FindByNameAsync(viewModel.UserName);

                    result = await _loginManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, viewModel.Checkbox, lockoutOnFailure: false);
                }
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuário, email ou senha inválidos");
                    return View();
                }
            }

            return View();
        }

        // Saída de usuários:
        [HttpGet("Sair")]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
                await _loginManager.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }

        // Usuário existe:
        public async Task<JsonResult> UserNameExist(string username, string Id)
        {
            if(string.IsNullOrEmpty(Id) && await _userManager.FindByNameAsync(username) != null)
                return Json("nome de usuário já cadastrado");
            else if(!string.IsNullOrEmpty(Id) && await _userManager.FindByNameAsync(username) != null)
            {
                var obj = await _userManager.FindByNameAsync(username);
                if(obj.Id != Id)
                    return Json("nome de usuário já cadastrado");
            }
                return Json(true);
        }

        public async Task<JsonResult> UserEmailExist(string email, string Id)
        {
            if (string.IsNullOrEmpty(Id) && await _userManager.FindByEmailAsync(email) != null)
                return Json("email já cadastrado");
            else if (!string.IsNullOrEmpty(Id) && await _userManager.FindByEmailAsync(email) != null)
            {
                var obj = await _userManager.FindByEmailAsync(email);
                if (obj.Id != Id)
                    return Json("nome de usuário já cadastrado");
            }
            return Json(true);
        }

        public async Task<JsonResult> UserCpfExist(string cpf, string Id)
        {
            if (await _userService.CpfExist(cpf, Id))
                return Json("CPF já cadastrado");
            return Json(true);
        }

        [HttpGet("Erro")]
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }

        [HttpGet("Atualizar")]
        public async Task<IActionResult> Edit(string Id)
        {
            if (Id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id nulo" });
            }

            var obj = await _userManager.FindByIdAsync(Id);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            }

            var viewModel = new RegisterFormViewModel { Id = obj.Id, Cpf = obj.Cpf, Email = obj.Email, EmailConf = obj.Email, Name = obj.Name, PhoneNumber = obj.PhoneNumber, UserName = obj.UserName };

            return View(viewModel);
        }

        [HttpPost("Atualizar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RegisterFormViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.Id);

            user.UserName = viewModel.UserName;
            user.Email = viewModel.Email;
            user.Cpf = viewModel.Cpf;
            user.PhoneNumber = viewModel.PhoneNumber;
            user.Name = viewModel.Name;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["confirm"] = user.UserName + " atualizado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            TempData["erro"] = "Não foi possível atualizar";
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpGet("Excluir")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (Id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id nulo" });
            }

            var obj = await _userManager.FindByIdAsync(Id);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            }

            return View(obj);
        }

        [Authorize]
        [HttpPost("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConf(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    TempData["confirm"] = user.Name + " foi deletado com sucesso.";
                    return RedirectToAction(nameof(Index), "Roles");
                }
                TempData["erro"] = "Não foi possível deletar o usuário";
                return RedirectToAction(nameof(Index), "Roles");
            }
            catch(DbUpdateException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        [Authorize]
        [HttpGet("Editar-nivel-de-acesso")]
        public async Task<IActionResult> EditRole(string Id)
        {
            if (Id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id nulo" });
            }

            var obj = await _userManager.FindByIdAsync(Id);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não encontrado" });
            }
            
            var list = await _roleManager.Roles.ToListAsync();
            ViewBag.roles = new SelectList(list, "Name", "Name");
            return View(obj);
        }

        [Authorize]
        [HttpPost("Editar-nivel-de-acesso")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userRole = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRoleAsync(user, userRole.FirstOrDefault());
            var result = await _userManager.AddToRoleAsync(user, role);

            if (result.Succeeded)
            {
                TempData["confirm"] = "Nível de acesso do usuário " + user.Name + " foi alterado com sucesso.";
                return RedirectToAction(nameof(Index), "Roles");
            }
            TempData["erro"] = "Não foi possível alterar o nível de acesso do usuário " + user.Name;
            return RedirectToAction(nameof(Index), "Roles");
        }
    }
}