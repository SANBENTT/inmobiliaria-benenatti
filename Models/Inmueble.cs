using System.ComponentModel.DataAnnotations;
namespace inmobiliaria_benenatti.Models;

public class Inmueble
{
         public int IdInmueble { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(255, ErrorMessage = "La dirección no puede superar los 255 caracteres")]
        public string Direccion { get; set; } = "";

        [Range(1, 20, ErrorMessage = "Los ambientes deben estar entre 1 y 20")]
        public int Ambientes { get; set; }

        [Range(1, 10000, ErrorMessage = "La superficie debe ser mayor a 0 y razonable")]
        public decimal Superficie { get; set; }

        [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90")]
        public decimal Latitud { get; set; }

        [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180")]
        public decimal Longitud { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un propietario")]
        public int PropietarioId { get; set; }

        public Propietarios? Propietario { get; set; }
}
