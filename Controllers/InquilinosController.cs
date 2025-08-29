using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_benenatti.Models;

namespace inmobiliaria_benenatti.Controllers;

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
        id = inquilino.id;
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

 
    public IActionResult Eliminar(int id)
    {
   
            repo.Baja(id);
        
    
        return RedirectToAction(nameof(Index));
    }
}
