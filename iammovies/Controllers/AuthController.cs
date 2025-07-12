
using iammovies.Models;
using iammovies.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace iammovies.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EmailService _emailService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        // GET: /Account/Login
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !user.EmailConfirmed)
            {
                ViewBag.ErrorMessage = "Email inválido ou não confirmado.";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                HttpContext.Session.SetString("UserName", user.Nome);
                HttpContext.Session.SetString("UserId", user.Id);
                HttpContext.Session.SetString("IsAdmin", user.Email == "admin@admin.com" ? "true" : "false");

                return Redirect(returnUrl ?? "/");
            }

            ViewBag.ErrorMessage = "Credenciais inválidas.";
            return View();
        }

        // GET: /Account/Register
        public IActionResult Register() => View();

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Nome,
                Email = model.Email,
                Nome = model.Nome,
                EmailConfirmado = false,
                TokenConfirmacao = Guid.NewGuid().ToString(),
                FotoPerfil = "" // Se usares avatar, define aqui
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var callbackUrl = Url.Action("ConfirmEmail", "Account",
                    new { token = user.TokenConfirmacao }, Request.Scheme);

                var mensagem = $"Olá {user.Nome},<br/>Clique para confirmar:<br/><a href='{callbackUrl}'>Confirmar Email</a>";
                await _emailService.SendEmailAsync(user.Email, "Confirmação de Email", mensagem);

                ViewBag.SucessoMensagem = $"Email enviado para {user.Email}.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        // GET: /Account/ConfirmEmail
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.TokenConfirmacao == token);
            if (user == null) return NotFound("Token inválido.");

            user.EmailConfirmado = true;
            user.TokenConfirmacao = null;
            await _userManager.UpdateAsync(user);

            ViewBag.ConfirmMessage = "Email confirmado com sucesso!";
            return View();
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
