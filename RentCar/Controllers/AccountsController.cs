using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentCar.Models;
using RentCar.Models.ViewModels;

namespace RentCar.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SignInManager<User> _loginManager;
        private readonly UserManager<User> _userManager;

        public AccountsController(SignInManager<User> loginManager, UserManager<User> userManager)
        {
            _loginManager = loginManager;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.GetUserAsync(User));
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
                   return LocalRedirect(returnUrl);
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
        public async Task<JsonResult> UserNameExist(string username)
        {
            if (await _userManager.FindByNameAsync(username) != null)
                return Json("nome de usuário já cadastrado");
            return Json(true);
        }

        public async Task<JsonResult> UserEmailExist(string email)
        {
            if (await _userManager.FindByEmailAsync(email) != null)
                return Json("email já cadastrado");
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
    }
}