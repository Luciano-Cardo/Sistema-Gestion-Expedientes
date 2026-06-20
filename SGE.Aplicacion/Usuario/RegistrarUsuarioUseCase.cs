using System.Security.Cryptography.X509Certificates;
using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Comun;
using SGE.Dominio.Entidades;
using SGE.Dominio.Entidades.Cifrado;
public class RegristrarUsuarioUseCase
{
    private readonly IUsuarioRepository _repoUsuario;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
   public RegristrarUsuarioUseCase(IUsuarioRepository repoUsuario, IUnidadDeTrabajo unidadDeTrabajo){
        _repoUsuario = repoUsuario;
        _unidadDeTrabajo = unidadDeTrabajo;
    }
   
   
    public RegistrarUsuarioResponse Ejecutar(RegistrarUsuarioRequest request)
    {
        if (_repoUsuario.obtenerPorCorreo(request.CorreoElectronico)!= null)
        {
            throw new DominioException("Ya existe un usuario con este correo");
        }
        ServicioHash sh = new ServicioHash();
        String contrasenaHasheada = sh.calcularHash(request.contrasena);
        Usuario usuario = new Usuario(request.Nombre,request.CorreoElectronico,contrasenaHasheada);
        _repoUsuario.Agregar(usuario);
        _unidadDeTrabajo.Guardar();
        return new RegistrarUsuarioResponse(usuario.Id);
    }
}