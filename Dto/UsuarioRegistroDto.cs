using System.ComponentModel.DataAnnotations;

namespace EmprestimoLivros.Dto
{
    public class UsuarioRegistroDto
    {
        [Required(ErrorMessage = "Digite o nome!")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Digite o sobrenome!")]
        public string Sobrenome { get; set; }
        [Required(ErrorMessage = "Digite o email!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Digite a senha!")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Digite a confirmação da senha!"),
        Compare("Senha", ErrorMessage = "As mensagens não estão iguais.")]
        public string ConfirmaSenha { get; set; }
    }
}

