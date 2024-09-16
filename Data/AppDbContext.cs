using EmprestimoLivros.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmprestimoLivros.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
        }
        public DbSet<EmprestimoModel> Emprestimos { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
    }
}
