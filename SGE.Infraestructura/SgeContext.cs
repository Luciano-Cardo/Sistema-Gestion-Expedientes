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

        modelBuilder.Entity<Expediente>().OwnsOne(c => c.Caratula, c =>{
            c.Property(p => p.Valor).HasColumnName("CaratulaValor");
            });
        modelBuilder.Entity<Tramite>().OwnsOne(c => c.Contenido, c =>{
            c.Property(p => p.Valor).HasColumnName("ContenidoTramite");
            });
    }
}