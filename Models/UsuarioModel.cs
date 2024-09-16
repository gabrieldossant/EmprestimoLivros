using Microsoft.AspNetCore.Mvc;

namespace EmprestimoLivros.Models
{
    public class UsuarioModel 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public byte[] SenhaHash { get; set; }
        public byte[] SenhaSalt { get; set; }
    }
}
