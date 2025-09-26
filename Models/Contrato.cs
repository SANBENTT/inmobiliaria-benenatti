using System.ComponentModel.DataAnnotations;
namespace inmobiliaria_benenatti.Models;

public class Contrato
{
    public int IdContrato { get; set; }
    [Required(ErrorMessage = "Debe seleccionar un inquilino")]
    public int InquilinoId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un inmueble")]
    public int InmuebleId { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
    [DataType(DataType.Date)]
    public DateTime FechaInicio { get; set; }

    [Required(ErrorMessage = "La fecha de fin es obligatoria")]
    [DataType(DataType.Date)]
    [DateGreaterThan("FechaInicio", ErrorMessage = "La fecha de fin debe ser mayor a la fecha de inicio")]
    public DateTime FechaFin { get; set; }

    [Range(1, 99999999, ErrorMessage = "El monto debe ser mayor a 0")]
    public decimal Monto { get; set; }


    public int? UsuarioCreadorId { get; set; }
    public int? UsuarioTerminadorId { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public DateTime? FechaTerminacion { get; set; }
    public bool Terminado { get; set; }
    public Inquilinos? Inquilino { get; set; }
    public Inmueble? Inmueble { get; set; }
    public Usuario? UsuarioCreador { get; set; }
    public Usuario? UsuarioTerminador { get; set; }

    public string DisplayInfo => $"{Inmueble?.Direccion} - {Inquilino?.nombre} ({FechaInicio:dd/MM/yyyy} a {FechaFin:dd/MM/yyyy})";
}