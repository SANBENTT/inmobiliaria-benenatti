using Microsoft.AspNetCore.Mvc;
using inmobiliaria_benenatti.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria_benenatti.Controllers
{
    [Authorize]
    public class PagosController : Controller
    {
        private readonly RepositorioPagos repositorio;
        private readonly RepositorioContratos repoContratos;
        private readonly IConfiguration configuration;

        public PagosController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorio = new RepositorioPagos();
            repoContratos = new RepositorioContratos();
        }

        public ActionResult Index()
        {
            try
            {
                var pagos = repositorio.ObtenerPagos();
                return View(pagos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<Pago>());
            }
        }

        public ActionResult Edicion(int id = 0)
        {
            var contratos = repoContratos.ObtenerContratos();
            ViewBag.Contratos = new SelectList(contratos, "IdContrato", "DisplayInfo");

            if (id == 0)
                return View(new Pago { FechaPago = DateTime.Today });
            else
                return View(repositorio.Obtener(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guardar(Pago pago)
        {
            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                if (usuarioActual != null)
                {
                    pago.UsuarioCreadorId = usuarioActual.IdUsuario;
                }

                if (ModelState.IsValid)
                {
                    if (pago.IdPago == 0)
                    {
                        repositorio.Alta(pago);
                        TempData["Success"] = "Pago creado exitosamente";
                    }
                    else
                    {
                        repositorio.Modificar(pago);
                        TempData["Success"] = "Pago modificado exitosamente";
                    }
                    return RedirectToAction(nameof(Index));
                }

                var contratos = repoContratos.ObtenerContratos();
                ViewBag.Contratos = new SelectList(contratos, "IdContrato", "DisplayInfo");
                return View("Edicion", pago);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                var contratos = repoContratos.ObtenerContratos();
                ViewBag.Contratos = new SelectList(contratos, "IdContrato", "DisplayInfo");
                return View("Edicion", pago);
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Anular(int id)
        {
            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                if (usuarioActual == null)
                {
                    TempData["Error"] = "No se pudo identificar al usuario";
                    return RedirectToAction(nameof(Index));
                }

                bool resultado = repositorio.AnularPago(id, usuarioActual.IdUsuario);
                if (resultado)
                {
                    TempData["Success"] = "Pago anulado de forma correcta";
                }
                else
                {
                    TempData["Error"] = "No se pudo anular el pago";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al anular el pago: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                var pago = repositorio.Obtener(id);
                if (pago == null)
                {
                    TempData["Error"] = "Pago no encontrado";
                    return RedirectToAction(nameof(Index));
                }

                int resultado = repositorio.Baja(id);
                if (resultado > 0)
                {
                    TempData["Success"] = "Pago eliminado correctamente";
                }
                else
                {
                    TempData["Error"] = "No se pudo eliminar el pago";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar el pago: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Detalles(int id)
        {
            var pago = repositorio.Obtener(id);
            if (pago == null)
            {
                return NotFound();
            }
            return View(pago);
        }

        public IActionResult PorContrato(int contratoId)
        {
            var pagos = repositorio.ObtenerPorContrato(contratoId);
            var contrato = repoContratos.Obtener(contratoId);
            ViewBag.Contrato = contrato;
            return View(pagos);
        }

        private Usuario? ObtenerUsuarioActual()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email)) return null;
            
            var repoUsuarios = new RepositorioUsuarios(configuration);
            return repoUsuarios.ObtenerPorEmail(email);
        }
    }
}