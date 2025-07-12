using iammovies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iammovies.Controllers
{
    public class FilmesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FilmesController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var filmesQuery = _context.Filmes.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                filmesQuery = filmesQuery.Where(f => f.Titulo.Contains(searchString));
            }

            var filmes = await filmesQuery.ToListAsync();

            var userId = _userManager.GetUserId(User);

            var favoritos = await _context.Favoritos
                .Where(f => f.UtilizadorId == userId)
                .Select(f => f.FilmeId)
                .ToListAsync();

            var viewModel = new FilmesViewModel
            {
                Filmes = filmes,
                FavoritosId = favoritos
            };

            return View(viewModel);
        }
    }
}