using SGE.Aplicacion.Autorizacion;
namespace SGE.Dominio.Entidades;
using System.Security.Cryptography;
using System.Text;
using SGE.Dominio.Comun;
using SGE.Dominio.Entidades;

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
                throw new DominioException("Todos los datos del usuario son obligatorios.");
            }        
        this.Id = Guid.NewGuid();
        this.Nombre = Nombre;
        this.CorreoElectronico = CorreoElectronico;
        this.ContrasenaHash = ContrasenaHash;
        this.esAdministrador = false;
        this.listaPermisos = new List<Permiso>();
    }




    public void ModificarDatos(string nombre, string? contrasenaHash)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new InvalidDataException("El nombre no puede estar vacío.");
        }

        this.Nombre = nombre;

        if (!string.IsNullOrWhiteSpace(contrasenaHash))
        {
            this.ContrasenaHash = contrasenaHash;
        }
    }

    public void AsignarPermiso(Permiso permiso)
    {
        if (!listaPermisos.Contains(permiso))
        {
            listaPermisos.Add(permiso);
        }
    }

    public void RemoverPermiso(Permiso permiso)
    {
        if (listaPermisos.Contains(permiso))
        {
            listaPermisos.Remove(permiso);
        }
    }

    public void convertirEnAdministrador()
    {
        this.esAdministrador = true;
    }
}