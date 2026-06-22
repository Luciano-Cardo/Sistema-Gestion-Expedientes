using SGE.Dominio.Entidades;
using SGE.Dominio.Autorizacion;
using SGE.Infraestructura.SQLite;

namespace SGE.Infraestructura;

public static class InicializadorBD
{
   public static void Inicializar(SgeContext context)
    {
        context.Database.EnsureCreated();

        if (context.Usuarios.Any())
        {
            return;
        }

        ServicioHash sh = new ServicioHash();

        var admin = new Usuario("Administrador Principal","admin@sge.com",sh.calcularHash("admin123"));
        admin.convertirEnAdministrador();

        var test1 = new Usuario("Usuario Prueba 1","prueba1@sge.com",sh.calcularHash("123456")); //con permisos
        test1.AsignarPermiso(Permiso.ExpedienteAlta);
        test1.AsignarPermiso(Permiso.TramiteAlta);

        var test2 = new Usuario("Usuario Prueba 2","prueba2@sge.com",sh.calcularHash("123456"));//sin permisos

        context.Usuarios.AddRange(admin,test1,test2);
        context.SaveChanges();
    }
}