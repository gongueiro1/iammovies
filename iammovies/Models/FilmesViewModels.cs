namespace iammovies.Models
{
    public class FilmesViewModel
    {
        public List<Filme> Filmes { get; set; } = new();
        public List<int> FavoritosId { get; set; } = new();
    }
}