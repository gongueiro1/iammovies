using System.ComponentModel.DataAnnotations;

namespace iammovies.Models
{
    public class Filme
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Descricao { get; set; }

        [MaxLength(100)]
        public string? Realizador { get; set; }

        public DateTime? DataLancamento { get; set; }

        public string? Capa { get; set; } // Aceita null
    }
}