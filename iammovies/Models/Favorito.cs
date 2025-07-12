using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iammovies.Models
{
    public class Favorito
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FilmeId { get; set; }

        [ForeignKey("FilmeId")]
        public Filme Filme { get; set; } = null!;

        [Required]
        public string UtilizadorId { get; set; } = string.Empty;

        [ForeignKey("UtilizadorId")]
        public ApplicationUser User { get; set; } = null!;
    }
}