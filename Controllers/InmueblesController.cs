using Microsoft.AspNetCore.Mvc;
using inmobiliaria_benenatti.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace inmobiliaria_benenatti.Controllers;

public class InmueblesController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private RepositorioInmuebles repo = new RepositorioInmuebles();
    private RepositorioPropietarios repoPropietarios = new RepositorioPropietarios();

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
        var propietarios = repoPropietarios.ObtenerListaPropietarios();
        ViewBag.Propietarios = new SelectList(propietarios, "id", "nombre");

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
        
        var propietarios = repoPropietarios.ObtenerListaPropietarios();
        ViewBag.Propietarios = new SelectList(propietarios, "id", "nombre");
        return View("Edicion", inmueble);
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
