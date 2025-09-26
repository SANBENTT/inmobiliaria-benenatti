using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_benenatti.Models;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria_benenatti.Controllers;
[Authorize]
public class PropietariosController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private RepositorioPropietarios repo = new RepositorioPropietarios();

    public PropietariosController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var lista = repo.obtenerPropietarios();
        return View(lista);
    }

    public IActionResult Edicion(int id)
    {
        if (id == 0)
            return View();
        else
        {
            var propietarios = repo.Obtener(id);
            return View(propietarios);
        }
    }

    [HttpPost]
    public IActionResult Guardar(int id, Propietarios propietarios)
    {
        if (!ModelState.IsValid)
        {
            return View("Edicion", propietarios);
        }
        if (repo.ExisteDni(propietarios.dni, propietarios.id))
        {
            ModelState.AddModelError("Dni", "Ya existe un inquilino con ese DNI");
            return View("Edicion", propietarios);
        }

        // Verificar email duplicado
        if (repo.ExisteEmail(propietarios.email!, id == 0 ? null : id))
        {
            ModelState.AddModelError("email", "El email ya está en uso por otro inquilino");
            return View("Edicion", propietarios);
        }

        if (id == 0)
        {
            repo.Alta(propietarios);
        }
        else
        {
            repo.Modificar(propietarios);
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
            var propietarios = repo.Obtener(id);
            if (propietarios == null)
            {
                return NotFound();
            }
            return View(propietarios);
        }
}