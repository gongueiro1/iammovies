
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iammovies.Models;
using System.IO;
using System.Threading.Tasks;

[Authorize]
public class PerfilController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly AppDbContext _context;

    public PerfilController(UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, AppDbContext context)
    {
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ApplicationUser updatedUser, IFormFile? novaFoto)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null) return NotFound();

        var nomeExiste = await _userManager.Users
            .AnyAsync(u => u.Nome == updatedUser.Nome && u.Id != user.Id);

        if (nomeExiste)
        {
            ModelState.AddModelError("Nome", "Este nome já está a ser utilizado por outro utilizador.");
            return View(user);
        }

        user.Nome = updatedUser.Nome;

        if (novaFoto != null && novaFoto.Length > 0)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(novaFoto.FileName);
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await novaFoto.CopyToAsync(stream);

            user.FotoPerfil = "/img/" + fileName;
        }

        await _userManager.UpdateAsync(user);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> EliminarConta()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToPage("/Account/Login", new { area = "Identity" });

        var favoritos = _context.Favoritos.Where(f => f.UtilizadorId == user.Id);
        _context.Favoritos.RemoveRange(favoritos);

        await _userManager.DeleteAsync(user);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }
}
