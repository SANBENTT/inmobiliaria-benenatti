using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_benenatti.Models;

public class Propietarios
{
    public int id { get; set; }

    [Required(ErrorMessage = "El DNI es obligatorio")]
    public string dni { get; set; } = "";

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
    public string nombre { get; set; } = "";

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [Phone(ErrorMessage = "El teléfono no tiene un formato válido")]
    public string telefono { get; set; } = "";

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
    public string? email { get; set; }

    [Required(ErrorMessage = "La dirección es obligatoria")]
    public string direccion { get; set; } = "";
}