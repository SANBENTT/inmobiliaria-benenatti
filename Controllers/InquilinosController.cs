using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_benenatti.Models;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria_benenatti.Controllers;
[Authorize]
public class InquilinosController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private RepositorioInquilinos repo = new RepositorioInquilinos();

    public InquilinosController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var lista = repo.ObtenerInquilinos();
        return View(lista);
    }
    public IActionResult Edicion(int id)
    {
        if (id == 0)
            return View();
        else
        {
            var inquilino = repo.Obtener(id);
            return View(inquilino);
        }
    }

    [HttpPost]
    public IActionResult Guardar(int id, Inquilinos inquilino)
    {
        if (!ModelState.IsValid)
        {
            return View("Edicion", inquilino);
        }
        if (repo.ExisteDni(inquilino.dni, inquilino.id))
        {
            ModelState.AddModelError("Dni", "Ya existe un inquilino con ese DNI");
            return View("Edicion", inquilino);
        }

        
        if (repo.ExisteEmail(inquilino.email!, id == 0 ? null : id))
        {
            ModelState.AddModelError("email", "El email ya está en uso por otro inquilino");
            return View("Edicion", inquilino);
        }

        if (id == 0)
        {
            repo.Alta(inquilino);
        }
        else
        {
            repo.Modificar(inquilino);
        }
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Eliminar(int id)
    {

        repo.Baja(id);


        return RedirectToAction(nameof(Index));

    }
    public IActionResult Detalles(int id)
        {
            var inquilino = repo.Obtener(id);
            if (inquilino == null)
            {
                return NotFound();
            }
            return View(inquilino);
        }
}
