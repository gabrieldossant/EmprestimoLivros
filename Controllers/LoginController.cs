using EmprestimoLivros.Dto;
using EmprestimoLivros.Services.LoginService;
using EmprestimoLivros.Services.SessaoService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace EmprestimoLivros.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginInterface _loginInterface;
        private readonly ISessaoInterface _sessaoInterface;
        public LoginController(ILoginInterface loginInterface, ISessaoInterface sessaoInterface) 
        { 
            _loginInterface = loginInterface;
            _sessaoInterface = sessaoInterface;
        }
        public IActionResult Login()
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public IActionResult Logout()
        {
            _sessaoInterface.RemoveSessao();
            return RedirectToAction("Login");
        }
        public IActionResult Registrar() 
        { 
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Registrar(UsuarioRegistroDto usuarioRegistroDto)
        {
            if (ModelState.IsValid)
            { 
                var usuario = await _loginInterface.RegistrarUsuario(usuarioRegistroDto);
                if(usuario.Status)
                {
                    TempData["MensagemSucesso"] = usuario.Mensagem;
                }
                else
                {
                    TempData["MensagemErro"] = usuario.Mensagem;
                    return View(usuarioRegistroDto);
                }
                return RedirectToAction("Home","Index");
            }
            else
            {
                return View(usuarioRegistroDto);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Login(UsuarioLoginDto usuarioLoginDto)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _loginInterface.Login(usuarioLoginDto);
                if (usuario.Status)
                {
                    TempData["MensagemSucesso"] = usuario.Mensagem;
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    TempData["MensagemErro"] = usuario.Mensagem;
                    return View(usuarioLoginDto);
                }
            }
            else
            {
                return View(usuarioLoginDto);
            }
        }
    }
}
