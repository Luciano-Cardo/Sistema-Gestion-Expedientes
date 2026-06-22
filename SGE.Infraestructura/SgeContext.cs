using Microsoft.EntityFrameworkCore;
using SGE.Dominio.Entidades;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
namespace SGE.Infraestructura.SQLite;

public class SgeContext : DbContext
{
    public DbSet<Usuario> Usuarios {get; set;}
    public DbSet<Expediente> Expedientes {get; set;}
    public DbSet<Tramite> Tramites {get; set;}
    public SgeContext(DbContextOptions<SgeContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Expediente>().ComplexProperty(e => e.Caratula);

        modelBuilder.Entity<Tramite>().ComplexProperty(t => t.Contenido);
    }
}