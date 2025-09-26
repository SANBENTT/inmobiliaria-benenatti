using Microsoft.AspNetCore.Mvc;
using inmobiliaria_benenatti.Models;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria_benenatti.Controllers
{
    [Authorize]
    public class TiposInmuebleController : Controller
    {
        private readonly RepositorioTiposInmueble repositorio;

        public TiposInmuebleController()
        {
            repositorio = new RepositorioTiposInmueble();
        }

        public ActionResult Index()
        {
            try
            {
                var tipos = repositorio.ObtenerTiposInmueble();
                return View(tipos);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<TipoInmueble>());
            }
        }

        public ActionResult Edicion(int id = 0)
        {
            if (id == 0)
                return View(new TipoInmueble());
            else
                return View(repositorio.ObtenerTipoInmueble(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guardar(TipoInmueble tipo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (tipo.IdTipoInmueble == 0)
                    {
                        repositorio.Alta(tipo);
                        TempData["Success"] = "Tipo de inmueble creado exitosamente";
                    }
                    else
                    {
                        repositorio.Modificar(tipo);
                        TempData["Success"] = "Tipo de inmueble modificado exitosamente";
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View("Edicion", tipo);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar el tipo de inmueble: {ex.Message}");
                return View("Edicion", tipo);
            }
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Eliminar(int id)
        {
            try
            {
                var tipo = repositorio.ObtenerTipoInmueble(id);
                if (tipo == null)
                {
                    TempData["Error"] = "Tipo de inmueble no encontrado";
                    return RedirectToAction(nameof(Index));
                }

                int resultado = repositorio.Baja(id);
                if (resultado > 0)
                {
                    TempData["Success"] = "Tipo de inmueble eliminado exitosamente";
                }
                else
                {
                    TempData["Error"] = "No se pudo eliminar el tipo de inmueble";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar el tipo de inmueble: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Detalles(int id)
        {
            var tipo = repositorio.ObtenerTipoInmueble(id);
            if (tipo == null)
            {
                return NotFound();
            }
            return View(tipo);
        }
    }
}