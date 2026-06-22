using SGE.Dominio.Autorizacion;
using SGE.Dominio.Comun;

namespace SGE.Dominio.Entidades;

public  class Usuario{
    public Guid Id { get; private set; }
    public String Nombre { get; private set; }
    public String CorreoElectronico { get; private set; }
    public String ContrasenaHash { get; private set; }
    public Boolean EsAdministrador { get; private set; }
    public List<Permiso> ListaPermisos { get; private set; } //SEGUN EDU ARRANCA VACIA
    
    protected Usuario(){ }
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
        this.EsAdministrador = false;
        this.ListaPermisos = new List<Permiso>();
    }

    public void ModificarDatos(string nombre, string? contrasenaHash)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new InvalidDataException("El nombre no puede estar vacío.");

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
        if (!ListaPermisos.Contains(permiso))
        {
            ListaPermisos.Add(permiso);
        }
    }

    public void RemoverPermiso(Permiso permiso)
    {
        if (ListaPermisos.Contains(permiso))
        {
            ListaPermisos.Remove(permiso);
        }
    }

    public void convertirEnAdministrador()
    {
        this.EsAdministrador = true;
    }

}