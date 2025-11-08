using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using inmobiliaria_benenatti.Models;

namespace inmobiliaria_benenatti.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContratosApiController : ControllerBase
    {
        private readonly RepositorioContratos repoContratos;
        private readonly RepositorioPagos repoPagos;
        private readonly RepositorioInmuebles repoInmuebles;

        public ContratosApiController()
        {
            repoContratos = new RepositorioContratos();
            repoPagos = new RepositorioPagos();
            repoInmuebles = new RepositorioInmuebles();
        }

        [HttpGet]
        public IActionResult ObtenerContratosPorPropietario()
        {
            try
            {
                var propietarioId = int.Parse(User.FindFirst("PropietarioId")!.Value);

                var inmuebles = repoInmuebles.ObtenerInmuebles()
                    .Where(i => i.PropietarioId == propietarioId)
                    .ToList();

                if (!inmuebles.Any())
                    return Ok(new { mensaje = "No se encontraron inmuebles para este propietario." });

                var resultado = new List<object>();

                foreach (var inmueble in inmuebles)
                {
                    var contratos = repoContratos.ObtenerPorInmueble(inmueble.IdInmueble);

                    var contratosConPagos = contratos.Select(c => new
                    {
                        contrato = c,
                        pagos = repoPagos.ObtenerPorContrato(c.IdContrato)
                    });

                    resultado.Add(new
                    {
                        inmueble = new
                        {
                            inmueble.IdInmueble,
                            inmueble.Direccion,
                            inmueble.Disponible
                        },
                        contratos = contratosConPagos
                    });
                }

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public IActionResult ObtenerContratoPorId(int id)
        {
            try
            {
                var propietarioId = int.Parse(User.FindFirst("PropietarioId")!.Value);

                var contrato = repoContratos.ObtenerPorId(id);
                if (contrato == null)
                    return NotFound(new { mensaje = "Contrato no encontrado." });

                var inmueble = repoInmuebles.Obtener(contrato.InmuebleId);
                if (inmueble == null || inmueble.PropietarioId != propietarioId)
                    return Unauthorized(new { mensaje = "No tiene permiso para ver este contrato." });

                var pagos = repoPagos.ObtenerPorContrato(id);

                return Ok(new
                {
                    contrato,
                    pagos
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}
