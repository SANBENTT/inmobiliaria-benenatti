using Microsoft.AspNetCore.Mvc;
using inmobiliaria_benenatti.Models;

namespace inmobiliaria_benenatti.Controllers;

public class InmueblesController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private RepositorioInmuebles repo = new RepositorioInmuebles();

    public InmueblesController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var lista = repo.ObtenerInmuebles();
        return View(lista);
    }

    public IActionResult Edicion(int id)
    {
        if (id == 0)
            return View();
        else
        {
            var inmueble = repo.Obtener(id);
            return View(inmueble);
        }
    }

    [HttpPost]
    public IActionResult Guardar(int id, Inmueble inmueble)
    {
        id = inmueble.IdInmueble;
        if (id == 0)
        {
            repo.Alta(inmueble);
        }
        else
        {
            repo.Modificar(inmueble);
        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Eliminar(int id)
    {
        repo.Baja(id);
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
}
