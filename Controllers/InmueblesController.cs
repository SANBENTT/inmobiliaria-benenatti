using Microsoft.AspNetCore.Mvc;
using inmobiliaria_benenatti.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria_benenatti.Controllers
{
    [Authorize]
    public class InmueblesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private RepositorioInmuebles repo = new RepositorioInmuebles();
        private RepositorioPropietarios repoPropietarios = new RepositorioPropietarios();
        private RepositorioTiposInmueble repoTipos = new RepositorioTiposInmueble();

        public InmueblesController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var lista = repo.ObtenerInmuebles();
            return View(lista);
        }

        public IActionResult Disponibles()
        {
            var inmueblesDisponibles = repo.ObtenerInmueblesDisponibles();
            return View(inmueblesDisponibles);
        }

        public IActionResult Edicion(int id)
        {
            CargarListas();
            
            if (id == 0)
                return View(new Inmueble());
            else
            {
                var inmueble = repo.Obtener(id);
                return View(inmueble);
            }
        }

        [HttpPost]
        public IActionResult Guardar(Inmueble inmueble)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (inmueble.IdInmueble == 0)
                    {
                        repo.Alta(inmueble);
                        TempData["Success"] = "Inmueble creado exitosamente";
                    }
                    else
                    {
                        repo.Modificar(inmueble);
                        TempData["Success"] = "Inmueble modificado exitosamente";
                    }
                    return RedirectToAction(nameof(Index));
                }

                CargarListas();
                return View("Edicion", inmueble);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar el inmueble: {ex.Message}");
                CargarListas();
                return View("Edicion", inmueble);
            }
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                if (repo.TieneContratosAsociados(id))
                {
                    int cantidadContratos = repo.CantidadContratosAsociados(id);
                    TempData["Error"] = $"No se puede eliminar el inmueble porque tiene {cantidadContratos} contrato(s) asociado(s). " +
                                       "Debe eliminar primero los contratos relacionados.";
                    return RedirectToAction(nameof(Index));
                }
                repo.Baja(id);
                TempData["Success"] = "Inmueble eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar el inmueble: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Detalles(int id)
        {
            var inmueble = repo.Obtener(id);
            if (inmueble == null)
            {
                return NotFound();
            }
            return View(inmueble);
        }

        public IActionResult DisponiblesPorFecha()
        {
            var modelo = new BusquedaFechasViewModel
            {
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(30)
            };
            return View("DisponiblesPorFecha", modelo);
        }

        [HttpPost]
        public IActionResult DisponiblesPorFecha(BusquedaFechasViewModel busqueda)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (busqueda.FechaFin <= busqueda.FechaInicio)
                    {
                        ModelState.AddModelError("FechaFin", "La fecha de fin debe ser mayor a la fecha de inicio");
                        return View(busqueda);
                    }

                    var inmueblesDisponibles = repo.ObtenerInmueblesDisponiblesPorFecha(busqueda.FechaInicio, busqueda.FechaFin);
                    
                    ViewBag.FechaInicio = busqueda.FechaInicio;
                    ViewBag.FechaFin = busqueda.FechaFin;
                    ViewBag.CantidadEncontrados = inmueblesDisponibles.Count;
                    
                    return View("ResultadosDisponiblesPorFecha", inmueblesDisponibles);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al buscar inmuebles: {ex.Message}");
                    return View(busqueda);
                }
            }
            
            return View(busqueda);
        }

        private void CargarListas()
        {
            var propietarios = repoPropietarios.ObtenerListaPropietarios();
            var tiposInmueble = repoTipos.ObtenerListaTiposInmueble();
            
            ViewBag.Propietarios = new SelectList(propietarios, "id", "nombre");
            ViewBag.TiposInmueble = new SelectList(tiposInmueble, "IdTipoInmueble", "Nombre");
            ViewBag.UsosInmueble = new SelectList(new[]
            {
                new { Value = (int)UsoInmueble.Residencial, Text = "Residencial" },
                new { Value = (int)UsoInmueble.Comercial, Text = "Comercial" }
            }, "Value", "Text");
        }
    }
}