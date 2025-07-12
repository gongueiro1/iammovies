using iammovies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace iammovies.Controllers
{
    [Authorize]
    public class FavoritosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritosController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Favoritos
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var favoritos = await _context.Favoritos
                .Where(f => f.UtilizadorId == userId)
                .Include(f => f.Filme)
                .ToListAsync();

            return View(favoritos);
        }

        // POST: /Favoritos/Adicionar
        [HttpPost]
        public async Task<IActionResult> Adicionar(int filmeId)
        {
            var userId = _userManager.GetUserId(User);

            // Evitar duplicados
            var jaExiste = await _context.Favoritos
                .AnyAsync(f => f.FilmeId == filmeId && f.UtilizadorId == userId);

            if (!jaExiste)
            {
                var favorito = new Favorito
                {
                    FilmeId = filmeId,
                    UtilizadorId = userId
                };

                _context.Favoritos.Add(favorito);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Filmes");
        }

        // POST: /Favoritos/Remover
        [HttpPost]
        public async Task<IActionResult> Remover(int filmeId)
        {
            var userId = _userManager.GetUserId(User);

            var favorito = await _context.Favoritos
                .FirstOrDefaultAsync(f => f.FilmeId == filmeId && f.UtilizadorId == userId);

            if (favorito != null)
            {
                _context.Favoritos.Remove(favorito);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Favoritos"); // <- aqui!
        }

    }
}
