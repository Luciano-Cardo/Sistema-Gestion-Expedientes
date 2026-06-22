using SGE.Dominio.Autorizacion;

namespace SGE.Dominio.Entidades;

public  class Usuario{
    public Guid Id { get; private set; }
    public String Nombre { get; private set; } = null!;
    public String CorreoElectronico { get; private set; } = null!;
    public String ContrasenaHash { get; private set; } = null!;
    public Boolean esAdministrador { get; private set; }
    public List<Permiso> listaPermisos { get; private set; } = new(); //SEGUN EDU ARRANCA VACIA
    
    protected Usuario(){ }
    public Usuario(String nombre, String correoElectronico, String contrasenaHash)
    {
         if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correoElectronico) || string.IsNullOrWhiteSpace(contrasenaHash))
            {
                throw new AutorizacionException("Todos los datos del usuario son obligatorios.");
            }        
        this.Id = Guid.NewGuid();
        this.Nombre = nombre;
        this.CorreoElectronico = correoElectronico;
        this.ContrasenaHash = contrasenaHash;
        this.esAdministrador = false;
        this.listaPermisos = new List<Permiso>();
    }


    // Factory Method para crear el usuario administrador (usado por ej. en el seed de datos)
    public static Usuario CrearAdministrador(String nombre, String correoElectronico, String contrasenaHash)
    {
        var usuario = new Usuario(nombre, correoElectronico, contrasenaHash);
        usuario.esAdministrador = true;
        return usuario;
    }

    public void ModificarDatos(string nombre, string? contrasenaHash)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new AutorizacionException("El nombre no puede estar vacío.");
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
}