using Microsoft.AspNetCore.Mvc;
using inmobiliaria_benenatti.Models;

namespace inmobiliaria_benenatti.Controllers
{
    public class ContratosController : Controller
    {
        private readonly RepositorioContratos repositorio;

        public ContratosController()
        {
            repositorio = new RepositorioContratos();
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
            try
            {
                if (id == 0)
                {

                    return View(new Contrato());
                }
                else
                {

                    var contrato = repositorio.Obtener(id);
                    if (contrato == null)
                    {
                        return NotFound();
                    }
                    return View(contrato);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new Contrato());
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guardar(Contrato contrato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (contrato.IdContrato == 0)
                    {

                        repositorio.Alta(contrato);
                        TempData["Success"] = "Contrato creado exitosamente";
                    }
                    else
                    {

                        repositorio.Modificar(contrato);
                        TempData["Success"] = "Contrato modificado exitosamente";
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View("Edicion", contrato);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Edicion", contrato);
            }
        }


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
        
    }
}