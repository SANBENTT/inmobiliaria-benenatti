using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_benenatti.Models
{
    public class TipoInmueble
    {
        public int IdTipoInmueble { get; set; }

        [Required(ErrorMessage = "El nombre del tipo es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        public string Nombre { get; set; } = "";

        [StringLength(255, ErrorMessage = "La descripción no puede superar los 255 caracteres")]
        public string? Descripcion { get; set; }

        public string DisplayName => $"{Nombre} {(Descripcion != null ? $"({Descripcion})" : "")}";
    }
}