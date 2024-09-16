using EmprestimoLivros.Data;
using EmprestimoLivros.Dto;
using EmprestimoLivros.Models;
using EmprestimoLivros.Services.SenhaService;
using EmprestimoLivros.Services.SessaoService;


namespace EmprestimoLivros.Services.LoginService
{
    public class LoginService : ILoginInterface
    {
        private readonly AppDbContext _context;
        private readonly ISenhaInterface _senhaInterface;
        private readonly ISessaoInterface _sessaoInterface;
        public LoginService(AppDbContext context, ISenhaInterface senhaInterface, ISessaoInterface sessaoInterface)
        {
            _context = context;
            _senhaInterface = senhaInterface;
            _sessaoInterface = sessaoInterface;
        }

        public async Task<ResponseModel<UsuarioModel>> Login(UsuarioLoginDto usuarioLoginDto)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == usuarioLoginDto.Email);
                if (usuario == null)
                {
                    response.Mensagem = "Credenciais Inválidas!";
                    response.Status = false;
                    return response;
                }
                if(!_senhaInterface.VerificaSenha(usuarioLoginDto.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                {
                    response.Mensagem = "Credenciais Inválidas!";
                    response.Status = false;
                    return response;
                }

                _sessaoInterface.CriarSessao(usuario);

                response.Mensagem = "Usuaário logado com sucesso!";
                return response;

            }
            catch (Exception ex)
            {
                response.Mensagem = "Credenciais Inválidas!";
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioRegistroDto usuarioRegistroDto)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {
                if(VerificarSeEmailExiste(usuarioRegistroDto))
                {
                    response.Mensagem = "Email Já cadastrado";
                    response.Status = false;
                    return response;
                }

                _senhaInterface.CriarSenhaHash(usuarioRegistroDto.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                var usuario = new UsuarioModel()
                {
                    Name = usuarioRegistroDto.Nome,
                    Sobrenome = usuarioRegistroDto.Sobrenome,
                    Email = usuarioRegistroDto.Email,
                    SenhaHash = senhaHash,
                    SenhaSalt = senhaSalt
                };
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                response.Mensagem = "Usuário cadastrado com sucesso";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }
        private bool VerificarSeEmailExiste(UsuarioRegistroDto usuarioRegistroDto)
        { 
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == usuarioRegistroDto.Email);
            if(usuario == null)
            {
                return false;
            }
            return true;
        }
    }
}
