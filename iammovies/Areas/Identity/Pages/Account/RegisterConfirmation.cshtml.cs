// RegisterConfirmation.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace iammovies.Areas.Identity.Pages.Account
{
    public class RegisterConfirmationModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        public void OnGet(string email)
        {
            Email = email;
        }
    }
}