using SGE.Dominio.Autorizacion;

namespace SGE.Aplicacion.Usuarios;

public record RegistrarUsuarioRequest(String CorreoElectronico, String Nombre, String contrasena);
public record RegistrarUsuarioResponse(Guid Id);

public record LoginUsuarioRequest(String CorreoElectronico, String contrasena);
public record LoginUsuarioResponse(String Token);

public record ModificarMisDatosRequest(Guid Id, String contrasena, String Nombre);

public record ListarUsuariosRequest(Guid Id);
public record UsuarioResumenResponse(Guid Id, String Nombre, String CorreoElectronico, Boolean EsAdministrador, List<Permiso> Permisos);
public record ListarUsuariosResponse(List<UsuarioResumenResponse> Usuarios);

public record EliminarUsuarioRequest(Guid IdOrigen, Guid IdAEliminar);

public record ModificarPermisosUsuarioRequest(Guid IdOrigen, Guid IdAEditar, Permiso nuevoPermiso, bool asignar);