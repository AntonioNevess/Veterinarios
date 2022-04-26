using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vets.Models;

namespace Vets.Data {
    /// <summary>
    /// esta classe funciona como base de dados do nosso projeto
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        /// <summary>
        /// este método é executado imediatamente antes da criação do Modelo
        /// É utilizado para dicionar as últimas instruções á criação do Modelo
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            //importa todo o comportamento do método, a quando da sua definição na SuperClasse
            base.OnModelCreating(modelBuilder);
            //adicionar registos que serão adicionaod sàs tabelas da BD
            modelBuilder.Entity<Veterinarios>().HasData(
                new Veterinarios() { Id = 1, Nome = "José Silva", NumCedulaProf = "vet-8765", Fotografia = "Jose.png" },
                new Veterinarios() { Id = 2, Nome = "Maria Gomes dos Santos", NumCedulaProf = "vet-6542", Fotografia = "Maria.png" },
                new Veterinarios() { Id = 3, Nome = "Ricardo Sousa", NumCedulaProf = "vet-1339", Fotografia = "Ricardo.png" }
                );
        
        }

        //definir as 'tabelas'
        public DbSet<Animais> Animais { get; set; }

        public DbSet<Veterinarios> Veterinarios { get; set; }

        public DbSet<Donos> Donos { get; set; }

        public DbSet<Consultas> Consultas { get; set; }

    }
}