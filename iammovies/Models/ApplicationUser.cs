using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace iammovies.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        public string FotoPerfil { get; set; } = "/img/default-profile.png";

        public bool EmailConfirmado { get; set; }

        public string TokenConfirmacao { get; set; } = string.Empty;
    }
}