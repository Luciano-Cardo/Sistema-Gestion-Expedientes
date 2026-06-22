using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Servicios;
using SGE.Dominio.Comun;
using SGE.Aplicacion.Interfaces;

namespace SGE.Aplicacion.Usuarios;

public class ModificarMisDatosUseCase
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IHashService _hashService;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public ModificarMisDatosUseCase(IUsuarioRepository usuarioRepo,IHashService hashService,IUnidadDeTrabajo unidadDeTrabajo)
    {
        _usuarioRepo = usuarioRepo;
        _hashService = hashService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public void Ejecutar(Guid Id, ModificarMisDatosRequest request)
    {
        if(Id != request.Id)
        {
            throw new DominioException("No puede modificar los datos de otro usuario.");
        }

        var usuario = _usuarioRepo.obtenerPorId(request.Id);
        if(usuario == null)
        {
            throw new EntidadNoEncontradaException("El usuario no existe.");
        }

        string? nuevaContrasenaHash = null;
        if(!string.IsNullOrWhiteSpace(request.contrasena))
        {
            nuevaContrasenaHash = _hashService.calcularHash(request.contrasena);
        }

        usuario.ModificarDatos(request.Nombre, nuevaContrasenaHash);

        _unidadDeTrabajo.GuardarCambios();
    }
}