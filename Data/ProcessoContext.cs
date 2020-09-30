using Microsoft.EntityFrameworkCore;
using MPSP.Models;

namespace MPSP.Data
{
    public class ProcessoContext : DbContext
    {
        public ProcessoContext(DbContextOptions<ProcessoContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Processo>()
                .HasIndex(p => new { p.NumeroPrefixo, p.Numero })
                .IsUnique();

            modelBuilder.Entity<Processo>()
                .HasMany(p => p.HistoricoProcesso)
                .WithOne(hp => hp.Processo);

            modelBuilder.Entity<Parte>()
                .HasIndex(p => new { p.Cpf })
                .IsUnique();

            modelBuilder.Entity<ProcessoParte>()
                .HasKey(bc => new { bc.ProcessoId, bc.ParteId });
            modelBuilder.Entity<ProcessoParte>()
                .HasOne(bc => bc.Processo)
                .WithMany(b => b.ProcessoPartes)
                .HasForeignKey(bc => bc.ProcessoId);
            modelBuilder.Entity<ProcessoParte>()
                .HasOne(bc => bc.Parte)
                .WithMany(c => c.ProcessoPartes)
                .HasForeignKey(bc => bc.ParteId);
        }

        public DbSet<Processo> Processo { get; set; }

        public DbSet<Parte> Parte { get; set; }

        public DbSet<ProcessoParte> ProcessoParte { get; set; }
    }
}
