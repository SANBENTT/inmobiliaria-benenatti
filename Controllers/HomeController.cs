using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobiliaria_benenatti.Models;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria_benenatti.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }


    [AllowAnonymous]
    public IActionResult DebugAuth()
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("<h1>Debug Authentication</h1>");
        sb.AppendLine($"<p>Authenticated: {User.Identity?.IsAuthenticated}</p>");
        sb.AppendLine($"<p>Name: {User.Identity?.Name ?? "null"}</p>");
        sb.AppendLine($"<p>AuthType: {User.Identity?.AuthenticationType ?? "null"}</p>");
        sb.AppendLine("<hr>");
        sb.AppendLine("<h3>Test Links:</h3>");
        sb.AppendLine("<ul>");
        sb.AppendLine("<li><a href='/Usuarios/Login'>Login Page</a></li>");
        sb.AppendLine("<li><a href='/Home/Protected'>Protected Page (should redirect to login)</a></li>");
        sb.AppendLine("</ul>");

        return Content(sb.ToString(), "text/html");
    }


    [Authorize]
    public IActionResult Protected()
    {
        return Content($"<h1>Protected Page</h1><p>Welcome {User.Identity?.Name}</p>", "text/html");
    }
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                ViewBag.Mensaje = $"¡Bienvenido {User.FindFirst("FullName")?.Value}!";
            }
            else
            {
                ViewBag.Mensaje = "Bienvenido a Inmobiliaria Benenatti";
            }

            return View();
        }



    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    

    
    
}
