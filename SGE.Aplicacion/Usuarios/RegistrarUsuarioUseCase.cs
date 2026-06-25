using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Servicios;
using SGE.Dominio.Entidades;

namespace SGE.Aplicacion.Usuarios; 

public class RegistrarUsuarioUseCase 
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IHashService _hashService; 
    private readonly IUnidadDeTrabajo _unidadDeTrabajo; 

    public RegistrarUsuarioUseCase(IUsuarioRepository usuarioRepository, IHashService hashService, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _usuarioRepository = usuarioRepository;
        _hashService = hashService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public RegistrarUsuarioResponse Ejecutar(RegistrarUsuarioRequest request)
    {
        string hash = _hashService.CalcularHash(request.Contrasena); 
        
        var nuevoUsuario = new Usuario(request.Nombre, request.CorreoElectronico, hash);
        
        _usuarioRepository.Agregar(nuevoUsuario);
        _unidadDeTrabajo.Guardar(); 

        return new RegistrarUsuarioResponse(nuevoUsuario.Id);
    }
}