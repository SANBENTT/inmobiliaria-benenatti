using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_benenatti.Models;

namespace inmobiliaria_benenatti.Controllers;

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
        id = propietarios.id;
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

 
    public IActionResult Eliminar(int id)
    {
   
            repo.Baja(id);
        
    
        return RedirectToAction(nameof(Index));
    }
}