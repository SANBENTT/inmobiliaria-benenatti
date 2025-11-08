using inmobiliaria_benenatti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_benenatti.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InmueblesApiController : ControllerBase
    {
        private readonly RepositorioInmuebles repo;
        private readonly IConfiguration configuration;

        public InmueblesApiController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repo = new RepositorioInmuebles();
        }

        [HttpGet]
        public IActionResult ObtenerInmuebles()
        {
            try
            {
                var propietarioId = int.Parse(User.FindFirst("PropietarioId")!.Value);
                var todos = repo.ObtenerInmuebles();
                var propios = todos.Where(x => x.PropietarioId == propietarioId).ToList();

                return Ok(propios);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerInmueble(int id)
        {
            try
            {
                var propietarioId = int.Parse(User.FindFirst("PropietarioId")!.Value);
                var inmueble = repo.Obtener(id);

                if (inmueble == null || inmueble.PropietarioId != propietarioId)
                    return NotFound(new { mensaje = "Inmueble no encontrado o no pertenece al propietario." });

                return Ok(inmueble);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CrearInmueble([FromBody] Inmueble inmueble)
        {
            try
            {
                var propietarioId = int.Parse(User.FindFirst("PropietarioId")!.Value);
                inmueble.PropietarioId = propietarioId;

                int nuevoId = repo.Alta(inmueble);
                inmueble.IdInmueble = nuevoId;

                return CreatedAtAction(nameof(ObtenerInmueble), new { id = nuevoId }, inmueble);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditarInmueble(int id, [FromBody] Inmueble datos)
        {
            try
            {
                var propietarioId = int.Parse(User.FindFirst("PropietarioId")!.Value);
                var inmuebleActual = repo.Obtener(id);

                if (inmuebleActual == null || inmuebleActual.PropietarioId != propietarioId)
                    return NotFound(new { mensaje = "No se puede editar un inmueble que no es tuyo." });

                inmuebleActual.Direccion = datos.Direccion ?? inmuebleActual.Direccion;
                inmuebleActual.Ambientes = datos.Ambientes != 0 ? datos.Ambientes : inmuebleActual.Ambientes;
                inmuebleActual.Superficie = datos.Superficie != 0 ? datos.Superficie : inmuebleActual.Superficie;
                inmuebleActual.Latitud = datos.Latitud != 0 ? datos.Latitud : inmuebleActual.Latitud;
                inmuebleActual.Longitud = datos.Longitud != 0 ? datos.Longitud : inmuebleActual.Longitud;
                inmuebleActual.TipoInmuebleId = datos.TipoInmuebleId != 0 ? datos.TipoInmuebleId : inmuebleActual.TipoInmuebleId;
                inmuebleActual.Uso = datos.Uso;
                inmuebleActual.Disponible = datos.Disponible;

                repo.Modificar(inmuebleActual);

                return Ok(new { mensaje = "Inmueble actualizado correctamente", inmueble = inmuebleActual });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarInmueble(int id)
        {
            try
            {
                var propietarioId = int.Parse(User.FindFirst("PropietarioId")!.Value);
                var inmueble = repo.Obtener(id);

                if (inmueble == null || inmueble.PropietarioId != propietarioId)
                    return NotFound(new { mensaje = "No se puede eliminar un inmueble que no es tuyo." });

                if (repo.TieneContratosAsociados(id))
                    return BadRequest(new { mensaje = "No se puede eliminar el inmueble porque tiene contratos asociados." });

                repo.Baja(id);
                return Ok(new { mensaje = "Inmueble eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("crear-con-foto")]
        public IActionResult CrearConFoto([FromForm] Inmueble inmueble, IFormFile? imagen)
        {
            try
            {
                var propietarioId = int.Parse(User.FindFirst("PropietarioId")!.Value);
                inmueble.PropietarioId = propietarioId;
                inmueble.Disponible = false;

                if (imagen != null && imagen.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Inmuebles");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imagen.CopyTo(fileStream);
                    }

                    inmueble.Foto = "/Uploads/Inmuebles/" + uniqueFileName;
                }

                int id = repo.Alta(inmueble);
                return Ok(new { mensaje = "Inmueble creado correctamente (por defecto deshabilitado)", id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPut("habilitar/{id}")]
        public IActionResult HabilitarInmueble(int id)
        {
            try
            {
                var propietarioId = int.Parse(User.FindFirst("PropietarioId")!.Value);
                var inmueble = repo.Obtener(id);

                if (inmueble == null || inmueble.PropietarioId != propietarioId)
                    return NotFound(new { mensaje = "Inmueble no encontrado o no pertenece al propietario." });

                inmueble.Disponible = true;
                repo.Modificar(inmueble);

                return Ok(new { mensaje = "Inmueble habilitado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPut("deshabilitar/{id}")]
        public IActionResult DeshabilitarInmueble(int id)
        {
            try
            {
                var propietarioId = int.Parse(User.FindFirst("PropietarioId")!.Value);
                var inmueble = repo.Obtener(id);

                if (inmueble == null || inmueble.PropietarioId != propietarioId)
                    return NotFound(new { mensaje = "Inmueble no encontrado o no pertenece al propietario." });

                inmueble.Disponible = false;
                repo.Modificar(inmueble);

                return Ok(new { mensaje = "Inmueble deshabilitado correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


    }
}