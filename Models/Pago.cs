using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_benenatti.Models;

public class Pago
{
    public int IdPago { get; set; }
    
    [Required(ErrorMessage = "Debe seleccionar un contrato")]
    public int ContratoId { get; set; }
    
    [Required(ErrorMessage = "La fecha de pago es obligatoria")]
    [DataType(DataType.Date)]
    public DateTime FechaPago { get; set; }
    
    [Required(ErrorMessage = "El monto es obligatorio")]
    [Range(1, 99999999, ErrorMessage = "El monto debe ser mayor a 0")]
    public decimal Monto { get; set; }
    
    [StringLength(500, ErrorMessage = "La observación no puede superar los 500 caracteres")]
    public string? Observacion { get; set; }
    public int? UsuarioCreadorId { get; set; }
    public int? UsuarioAnuladorId { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public DateTime? FechaAnulacion { get; set; }
    public bool Anulado { get; set; }
    public Contrato? Contrato { get; set; }
    public Usuario? UsuarioCreador { get; set; }
    public Usuario? UsuarioAnulador { get; set; }
}