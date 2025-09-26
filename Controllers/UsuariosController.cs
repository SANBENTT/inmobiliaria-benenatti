using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using inmobiliaria_benenatti.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace inmobiliaria_benenatti.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RepositorioUsuarios repo;
        private readonly IWebHostEnvironment hostingEnvironment;

        public UsuariosController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            this.configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
            repo = new RepositorioUsuarios(configuration);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int id = repo.Alta(usuario);
                    TempData["Success"] = "Usuario creado correctamente";
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al crear usuario: {ex.Message}";
            }

            return View(usuario);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                if (string.IsNullOrEmpty(login.Usuario))
                {
                    ModelState.AddModelError("Usuario", "El email es requerido");
                    return View(login);
                }

                var returnUrl = string.IsNullOrEmpty(TempData["returnUrl"]?.ToString())
                    ? "/Home"
                    : TempData["returnUrl"].ToString();

                if (ModelState.IsValid)
                {
                    var saltValue = configuration["Salt"];
                    if (string.IsNullOrEmpty(saltValue))
                    {
                        ModelState.AddModelError("", "Configuración de Salt no encontrada");
                        return View(login);
                    }

                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(saltValue),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 5000,
                        numBytesRequested: 256 / 8));

                    var usuario = repo.ObtenerPorEmail(login.Usuario);

                    if (usuario == null || usuario.Clave != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        return View(login);
                    }

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim("FullName", usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.RolNombre),
            };

                    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                    await HttpContext.SignInAsync(
                        "Cookies",
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                        });

                    TempData.Remove("returnUrl");
                    return RedirectToAction("Index", "Home");
                }

                TempData["returnUrl"] = returnUrl;
                return View(login);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Sucedio un error en el proceso de login.");
                return View(login);
            }
        }



        [Authorize]
        public IActionResult Perfil()
        {
            var email = User.Identity?.Name;
            var usuario = repo.ObtenerPorEmail(email);

            if (usuario == null)
            {
                return RedirectToAction("Logout");
            }

            return View(usuario);
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditarPerfil()
        {
            var email = User.Identity?.Name;
            var usuario = repo.ObtenerPorEmail(email);

            if (usuario == null)
            {
                return RedirectToAction("Logout");
            }

            ViewBag.RolActual = usuario.RolNombre;
            return View(usuario);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditarPerfil(Usuario usuario)
        {
            try
            {
                var usuarioActual = repo.ObtenerPorEmail(User.Identity.Name);
                if (usuarioActual == null)
                {
                    return RedirectToAction("Logout");
                }

                usuario.Rol = usuarioActual.Rol;
                usuario.IdUsuario = usuarioActual.IdUsuario;

                if (!string.IsNullOrEmpty(usuario.NuevaClave))
                {
                    var saltValue = configuration["Salt"];
                    string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: usuario.NuevaClave,
                        salt: System.Text.Encoding.ASCII.GetBytes(saltValue),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 5000,
                        numBytesRequested: 256 / 8));

                    usuario.Clave = hashedPassword;
                }
                else
                {
                    usuario.Clave = usuarioActual.Clave;
                }

                if (usuario.AvatarFile != null && usuario.AvatarFile.Length > 0)
                {
                    usuario.Avatar = await GuardarAvatar(usuario.AvatarFile, usuario.IdUsuario);
                }
                else if (string.IsNullOrEmpty(usuario.Avatar))
                {
                    usuario.Avatar = null;
                    EliminarAvatar(usuarioActual.Avatar);
                }
                else
                {
                    usuario.Avatar = usuarioActual.Avatar;
                }

                repo.ActualizarUsuario(usuario);

                if (usuarioActual.Nombre != usuario.Nombre)
                {
                    await ActualizarClaims(usuario);
                }

                TempData["Success"] = "Perfil actualizado correctamente";
                return RedirectToAction("Perfil");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al actualizar perfil: {ex.Message}";
                return View(usuario);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult EliminarAvatar()
        {
            try
            {
                var usuario = repo.ObtenerPorEmail(User.Identity.Name);
                if (usuario == null)
                {
                    return RedirectToAction("Logout");
                }

                EliminarAvatar(usuario.Avatar);

                usuario.Avatar = null;
                repo.ActualizarUsuario(usuario);

                TempData["Success"] = "Avatar eliminado correctamente";
                return RedirectToAction("Perfil");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar avatar: {ex.Message}";
                return RedirectToAction("Perfil");
            }
        }

        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            TempData["Success"] = "Sesion cerrada correctamente";
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            var usuarios = repo.ObtenerTodos();
            return View(usuarios);
        }


        private async Task<string> GuardarAvatar(IFormFile avatarFile, int usuarioId)
        {
            if (avatarFile == null || avatarFile.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads", "avatars");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"avatar_{usuarioId}_{Guid.NewGuid()}{Path.GetExtension(avatarFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await avatarFile.CopyToAsync(stream);
            }

            return $"/uploads/avatars/{fileName}";
        }

        private void EliminarAvatar(string avatarPath)
        {
            if (!string.IsNullOrEmpty(avatarPath))
            {
                var fullPath = Path.Combine(hostingEnvironment.WebRootPath, avatarPath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
        }

        private async Task ActualizarClaims(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim("FullName", usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.RolNombre),
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("Cookies", claimsPrincipal);
        }
        


                [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var usuarios = repo.ObtenerTodos();
            var usuario = usuarios.FirstOrDefault(u => u.IdUsuario == id);
            
            if (usuario == null)
            {
                TempData["Error"] = "Usuario no encontrado";
                return RedirectToAction("Index");
            }

            if (usuario.Email == User.Identity.Name)
            {
                TempData["Error"] = "Use la opcion 'Mi Perfil' para editar su informacion propia";
                return RedirectToAction("Index");
            }

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Usuario usuario)
        {
            try
            {
                bool cambiarClave = !string.IsNullOrEmpty(usuario.NuevaClave);

                if (cambiarClave && usuario.NuevaClave != usuario.ConfirmarClave)
                {
                    ModelState.AddModelError("NuevaClave", "Las contraseñas no coinciden.");
                    return View(usuario);
                }

                repo.ActualizarUsuario(usuario, cambiarClave);
                TempData["Success"] = "Usuario actualizado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(usuario);
            }
        }



        [Authorize(Roles = "Administrador")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                var usuario = repo.ObtenerTodos().FirstOrDefault(u => u.IdUsuario == id);
                if (usuario == null)
                {
                    TempData["Error"] = "Usuario no encontrado";
                    return RedirectToAction("Index");
                }

                if (usuario.Email == User.Identity.Name)
                {
                    TempData["Error"] = "No puedes eliminar tu propio usuario";
                    return RedirectToAction("Index");
                }

                if (!string.IsNullOrEmpty(usuario.Avatar))
                {
                    EliminarAvatar(usuario.Avatar);
                }

                int filas = repo.Baja(id);
                if (filas > 0)
                {
                    TempData["Success"] = "Usuario eliminado correctamente";
                }
                else
                {
                    TempData["Error"] = "No se elimino ningún usuario";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar usuario: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

    }
}