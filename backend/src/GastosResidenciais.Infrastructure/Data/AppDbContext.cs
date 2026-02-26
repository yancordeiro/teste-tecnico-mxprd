using GastosResidenciais.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GastosResidenciais.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuracao da entidade Pessoa
            modelBuilder.Entity<Pessoa>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Nome).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Idade).IsRequired();
            });

            // Configuracao da entidade Categoria
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Descricao).IsRequired().HasMaxLength(400);
                entity.Property(c => c.Finalidade).IsRequired();
            });

            // Configuracao da entidade Transacao
            modelBuilder.Entity<Transacao>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Descricao).IsRequired().HasMaxLength(400);
                entity.Property(t => t.Valor).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(t => t.Tipo).IsRequired();

                // Relacionamento com Categoria
                entity.HasOne(t => t.Categoria)
                    .WithMany(c => c.Transacoes)
                    .HasForeignKey(t => t.CategoriaId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relacionamento com Pessoa
                entity.HasOne(t => t.Pessoa)
                    .WithMany(p => p.Transacoes)
                    .HasForeignKey(t => t.PessoaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
