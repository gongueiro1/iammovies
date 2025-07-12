
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace iammovies.Models
{
    public class IdentityDb : IdentityDbContext<ApplicationUser>
    {
        public IdentityDb(DbContextOptions<IdentityDb> options)
            : base(options)
        {
        }
    }
}
