using SGE.Dominio.Autorizacion;
using SGE.Dominio.Comun;

namespace SGE.Dominio.Entidades;

public  class Usuario{
    public Guid Id { get; private set; }
    public String Nombre { get; private set; }
    public String CorreoElectronico { get; private set; }
    public String ContrasenaHash { get; private set; }
    public Boolean EsAdministrador { get; private set; }

    private readonly List<Permiso> _listaPermisos = new();
    public IReadOnlyCollection<Permiso> ListaPermisos => _listaPermisos;

    protected Usuario(){ }
    public Usuario(String nombre, String correoElectronico, String contrasenaHash)
    {
         if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correoElectronico) || string.IsNullOrWhiteSpace(contrasenaHash))
            {
                throw new DominioException("Todos los datos del usuario son obligatorios.");
            }        
        this.Id = Guid.NewGuid();
        this.Nombre = nombre;
        this.CorreoElectronico = correoElectronico;
        this.ContrasenaHash = contrasenaHash;
        this.EsAdministrador = false;
    }

    public void ModificarDatos(string nombre, string? contrasenaHash)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new DominioException("El nombre no puede estar vacío.");
        }

        this.Nombre = nombre;

        if (!string.IsNullOrWhiteSpace(contrasenaHash))
        {
            this.ContrasenaHash = contrasenaHash;
        }
    }

    public void AsignarPermiso(Permiso permiso)
    {
        if (!_listaPermisos.Contains(permiso))
        {
            _listaPermisos.Add(permiso);
        }
    }

    public void RemoverPermiso(Permiso permiso)
    {
        if (_listaPermisos.Contains(permiso))
        {
            _listaPermisos.Remove(permiso);
        }
    }

    public void convertirEnAdministrador()
    {
        this.EsAdministrador = true;
    }

}