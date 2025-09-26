using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace inmobiliaria_benenatti.Models
{
    public enum enRoles
    {
        Administrador = 1,  
        Empleado = 2          
    }

    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Clave { get; set; }

        [Required]
        public string? Nombre { get; set; }

        [Required]
        public int Rol { get; set; } = (int)enRoles.Empleado;

        [NotMapped]
        public string RolNombre => Rol > 0 ? ((enRoles)Rol).ToString() : "";

        public string? Avatar { get; set; }
        [NotMapped] 
        [DataType(DataType.Upload)]
        public IFormFile? AvatarFile { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        public string? NuevaClave { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("NuevaClave", ErrorMessage = "Las contraseñas no coinciden")]
        public string? ConfirmarClave { get; set; }

        public DateTime CreadoEn { get; set; }

        public static IDictionary<int, string> ObtenerRoles()
        {
            return new Dictionary<int, string>
            {
                { (int)enRoles.Administrador, "Administrador" },
                { (int)enRoles.Empleado, "Empleado" }
            };
        }
    }
}