using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using iammovies.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace iammovies.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public AdminController(AppDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // ------------------ HOME ADMIN ------------------
        public IActionResult Index()
        {
            return View();
        }

        // ------------------ FILMES ------------------

        [HttpGet]
        public async Task<IActionResult> GerirFilmes()
        {
            var filmes = await _context.Filmes.ToListAsync();
            return View(filmes);
        }

        [HttpGet]
        public IActionResult AdicionarFilme()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarFilme(Filme filme, IFormFile? capa)
        {
            if (!ModelState.IsValid)
                return View(filme);

            if (capa != null && capa.Length > 0)
            {
                var pastaDestino = Path.Combine(_env.WebRootPath, "img");
                if (!Directory.Exists(pastaDestino))
                    Directory.CreateDirectory(pastaDestino);

                var nomeFicheiro = Path.GetFileName(capa.FileName);
                var caminhoCompleto = Path.Combine(pastaDestino, nomeFicheiro);

                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    await capa.CopyToAsync(stream);
                }

                filme.Capa = nomeFicheiro;
            }
            else
            {
                filme.Capa = "semimagem.jpg"; // Se não for enviada imagem
            }

            _context.Filmes.Add(filme);
            await _context.SaveChangesAsync();

            return RedirectToAction("GerirFilmes");
        }

        [HttpGet]
        public async Task<IActionResult> EditarFilme(int id)
        {
            var filme = await _context.Filmes.FindAsync(id);
            if (filme == null)
                return NotFound();

            return View(filme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarFilme(Filme filme, IFormFile? novaCapa)
        {
            if (!ModelState.IsValid)
                return View(filme);

            var filmeExistente = await _context.Filmes.FindAsync(filme.Id);
            if (filmeExistente == null)
                return NotFound();

            filmeExistente.Titulo = filme.Titulo;
            filmeExistente.Descricao = filme.Descricao;
            filmeExistente.DataLancamento = filme.DataLancamento;
            filmeExistente.Realizador = filme.Realizador;

            if (novaCapa != null && novaCapa.Length > 0)
            {
                var pastaDestino = Path.Combine(_env.WebRootPath, "img");
                if (!Directory.Exists(pastaDestino))
                    Directory.CreateDirectory(pastaDestino);

                var nomeFicheiro = Path.GetFileName(novaCapa.FileName);
                var caminhoCompleto = Path.Combine(pastaDestino, nomeFicheiro);

                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    await novaCapa.CopyToAsync(stream);
                }

                filmeExistente.Capa = nomeFicheiro;
            }

            _context.Update(filmeExistente);
            await _context.SaveChangesAsync();

            return RedirectToAction("GerirFilmes");
        }

        [HttpGet]
        public async Task<IActionResult> ApagarFilme(int id)
        {
            var filme = await _context.Filmes.FindAsync(id);
            if (filme == null)
                return NotFound();

            _context.Filmes.Remove(filme);
            await _context.SaveChangesAsync();

            return RedirectToAction("GerirFilmes");
        }

        // ------------------ UTILIZADORES ------------------

        [HttpGet]
        public async Task<IActionResult> GerirUtilizadores()
        {
            var utilizadores = await _userManager.Users.ToListAsync();
            return View(utilizadores);
        }

        // ------------------ ESTATÍSTICAS ------------------

        [HttpGet]
        public async Task<IActionResult> Estatisticas()
        {
            ViewBag.TotalUsers = await _userManager.Users.CountAsync();
            ViewBag.TotalFilmes = await _context.Filmes.CountAsync();
            ViewBag.TotalFavoritos = await _context.Favoritos.CountAsync();
            return View();
        }
    }
}
