using SGE.Aplicacion.Autorizacion;
namespace SGE.Dominio.Entidades;
using System.Security.Cryptography;
using System.Text;
using SGE.Dominio.Entidades.Cifrado;

public  class Usuario{
    public Guid Id { get; private set; }
    public String Nombre { get; private set; }
    public String CorreoElectronico { get; private set; }
    public String ContrasenaHash { get; private set; }

    public Boolean esAdministrador { get; private set; }
    public List<Permiso> listaPermisos { get; private set; } //SEGUN EDU ARRANCA VACIA
   
   
   
   
   
   
   
    public Usuario(String Nombre, String CorreoElectronico, String ConstrasenaHash)
    {
         if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(CorreoElectronico) || string.IsNullOrWhiteSpace(ContrasenaHash))
            {
                throw new AutorizacionException("Todos los datos del usuario son obligatorios.");
            }        
        this.Id = Guid.NewGuid();
        this.Nombre = Nombre;
        this.CorreoElectronico = CorreoElectronico;
        this.ContrasenaHash = ContrasenaHash;
        this.esAdministrador = false;
        this.listaPermisos = new List<Permiso>();
    }

}