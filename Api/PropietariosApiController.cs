using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using inmobiliaria_benenatti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace inmobiliaria_benenatti.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PropietariosApiController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioPropietarios repo;

        public PropietariosApiController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repo = new RepositorioPropietarios();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromForm] string email, [FromForm] string clave)
        {
            var propietario = repo.ObtenerPorEmail(email);
            if (propietario == null || propietario.clave != clave)
            {
                return Unauthorized(new { mensaje = "Email o clave incorrectos" });
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, propietario.email!),
                new Claim("PropietarioId", propietario.id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["TokenAuthentication:Issuer"],
                audience: configuration["TokenAuthentication:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                propietario = new
                {
                    propietario.id,
                    propietario.nombre,
                    propietario.email
                }
            });
        }

        [HttpGet("perfil")]
        public IActionResult ObtenerPerfil()
        {
            try
            {
                var propietarioIdClaim = User.FindFirst("PropietarioId")?.Value;

                if (propietarioIdClaim == null)
                    return Unauthorized("Token inválido o sin ID de propietario.");

                int propietarioId = int.Parse(propietarioIdClaim);

                var propietario = repo.Obtener(propietarioId);
                if (propietario == null)
                    return NotFound("Propietario no encontrado.");

                return Ok(propietario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


   [HttpPut("perfil")]
public IActionResult EditarPerfil([FromBody] PropietarioEditarRequest datos)
{
    try
    {
        var propietarioIdClaim = User.FindFirst("PropietarioId")?.Value;
        if (propietarioIdClaim == null)
            return Unauthorized("Token inválido o sin ID de propietario.");

        int propietarioId = int.Parse(propietarioIdClaim);
        var propietarioActual = repo.Obtener(propietarioId);

        if (propietarioActual == null)
            return NotFound("Propietario no encontrado.");

        if (!string.IsNullOrEmpty(datos.Nombre))
            propietarioActual.nombre = datos.Nombre;
        if (!string.IsNullOrEmpty(datos.Telefono))
            propietarioActual.telefono = datos.Telefono;
        if (!string.IsNullOrEmpty(datos.Direccion))
            propietarioActual.direccion = datos.Direccion;

        repo.Modificar(propietarioActual);

        return Ok(new
        {
            mensaje = "Perfil actualizado correctamente",
            propietario = propietarioActual
        });
    }
    catch (Exception ex)
    {
        return BadRequest(new { error = ex.Message });
    }
}

        public class PropietarioEditarRequest
        {
            public string? Nombre { get; set; }
            public string? Telefono { get; set; }
            public string? Direccion { get; set; }
        }


        [HttpPut("cambiar-clave")]
        public IActionResult CambiarClave([FromBody] CambioClaveRequest request)
        {
            try
            {
                var propietarioIdClaim = User.FindFirst("PropietarioId")?.Value;
                if (propietarioIdClaim == null)
                    return Unauthorized("Token inválido o sin ID de propietario.");

                int propietarioId = int.Parse(propietarioIdClaim);
                var propietario = repo.Obtener(propietarioId);

                if (propietario == null)
                    return NotFound("Propietario no encontrado.");

                if (propietario.clave != request.ClaveActual)
                    return Unauthorized(new { mensaje = "La clave actual no es correcta." });

                propietario.clave = request.NuevaClave;
                repo.Modificar(propietario);

                return Ok(new { mensaje = "Contraseña actualizada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class CambioClaveRequest
    {
        public string ClaveActual { get; set; } = "";
        public string NuevaClave { get; set; } = "";
    }
}



    
