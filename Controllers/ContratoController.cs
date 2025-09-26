using Microsoft.AspNetCore.Mvc;
using inmobiliaria_benenatti.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria_benenatti.Controllers
{
    [Authorize]
    public class ContratosController : Controller
    {
        private readonly RepositorioContratos repositorio;
        private readonly RepositorioInmuebles repoInmuebles;
        private readonly RepositorioInquilinos repoInquilinos;
        private readonly IConfiguration configuration;

        public ContratosController(IConfiguration configuration)
        {
            this.configuration = configuration;
            repositorio = new RepositorioContratos();
            repoInmuebles = new RepositorioInmuebles();
            repoInquilinos = new RepositorioInquilinos();
        }

        public ActionResult Index()
        {
            try
            {
                var contratos = repositorio.ObtenerContratos();
                return View(contratos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<Contrato>());
            }
        }

        public ActionResult Edicion(int id = 0)
        {
            CargarListas();
            
            if (id == 0)
                return View(new Contrato());
            else
                return View(repositorio.Obtener(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guardar(Contrato contrato)
        {
            try
            {
                
                var usuarioActual = ObtenerUsuarioActual();
                if (usuarioActual != null)
                {
                    contrato.UsuarioCreadorId = usuarioActual.IdUsuario;
                }

                
                if (ExisteSuperposicionContrato(contrato))
                {
                    ModelState.AddModelError("", "El inmueble ya tiene un contrato activo en las fechas seleccionadas.");
                }

                if (ModelState.IsValid)
                {
                    if (contrato.IdContrato == 0)
                    {
                        repositorio.Alta(contrato);
                        TempData["Success"] = "Contrato creado Correctamente";
                    }
                    else
                    {
                        repositorio.Modificar(contrato);
                        TempData["Success"] = "Cambios Guardados";
                    }
                    return RedirectToAction(nameof(Index));
                }
                
                CargarListas();
                return View("Edicion", contrato);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar el contrato: {ex.Message}");
                CargarListas();
                return View("Edicion", contrato);
            }
        }

        
        [Authorize(Roles = "Administrador,Empleado")]
        public ActionResult Terminar(int id)
        {
            try
            {
                var usuarioActual = ObtenerUsuarioActual();
                if (usuarioActual == null)
                {
                    TempData["Error"] = "No se pudo identificar al usuario";
                    return RedirectToAction(nameof(Index));
                }

                bool resultado = repositorio.TerminarContrato(id, usuarioActual.IdUsuario);
                if (resultado)
                {
                    TempData["Success"] = "Contrato finalizado exitosamente";
                }
                else
                {
                    TempData["Error"] = "No se pudo finalizar el contrato";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al terminar el contrato: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        
        private Usuario? ObtenerUsuarioActual()
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email)) return null;
            
            var repoUsuarios = new RepositorioUsuarios(configuration);
            return repoUsuarios.ObtenerPorEmail(email);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                var contrato = repositorio.Obtener(id);
                if (contrato == null)
                {
                    TempData["Error"] = "Contrato no encontrado";
                    return RedirectToAction(nameof(Index));
                }

                int resultado = repositorio.Baja(id);
                if (resultado > 0)
                {
                    TempData["Success"] = "Contrato eliminado exitosamente";
                }
                else
                {
                    TempData["Error"] = "No se pudo eliminar el contrato";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar el contrato: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Detalles(int id)
        {
            var contrato = repositorio.Obtener(id);
            if (contrato == null)
            {
                return NotFound();
            }
            return View(contrato);
        }

        
        private void CargarListas()
        {
            ViewBag.Inmuebles = new SelectList(repoInmuebles.ObtenerListaInmuebles(), "IdInmueble", "Direccion");
            ViewBag.Inquilinos = new SelectList(repoInquilinos.ObtenerListaInquilinos(), "id", "nombre");
        }

        private bool ExisteSuperposicionContrato(Contrato contrato)
        {
            return repositorio.ExisteSuperposicionContrato(
                contrato.InmuebleId, 
                contrato.FechaInicio, 
                contrato.FechaFin, 
                contrato.IdContrato
            );
        }
    }
}