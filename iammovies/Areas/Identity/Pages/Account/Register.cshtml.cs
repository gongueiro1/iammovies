using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using iammovies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace iammovies.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext                 _context;
        private readonly IEmailSender                 _emailSender;
        private readonly ILogger<RegisterModel>       _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            AppDbContext context,
            IEmailSender emailSender,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _context     = context;
            _emailSender = emailSender;
            _logger      = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string ReturnUrl { get; set; } = string.Empty;

        public class InputModel
        {
            [Required, Display(Name = "Nome")]
            public string Nome { get; set; } = string.Empty;

            [Required, EmailAddress, Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Required, DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "As passwords não coincidem.")]
            [Display(Name = "Confirmar Password")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet(string returnUrl = null)
            => ReturnUrl = returnUrl ?? Url.Content("~/");

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (!ModelState.IsValid)
                return Page();

            if (await _context.Users.AnyAsync(u => u.Nome == Input.Nome))
            {
                ModelState.AddModelError("Input.Nome", "Este nome de utilizador já está em uso.");
                return Page();
            }

            var user = new ApplicationUser
            {
                UserName   = Input.Email,
                Email      = Input.Email,
                Nome       = Input.Nome,
                FotoPerfil = "img/default-profile.png"
            };

            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("Utilizador criado; a enviar confirmação de e-mail.");

                var token       = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var code        = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values:     new { area = "Identity", userId = user.Id, code },
                    protocol:   Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Confirma o teu e-mail",
                    $"Olá {Input.Nome}, confirma <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>aqui</a>.");

                return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
            }

            foreach (var e in result.Errors)
                ModelState.AddModelError(string.Empty, e.Description);

            return Page();
        }
    }
}
