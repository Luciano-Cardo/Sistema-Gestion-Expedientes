using SGE.Dominio.Autorizacion;

namespace SGE.Aplicacion.Usuarios;

public record RegistrarUsuarioRequest(string CorreoElectronico, string Nombre, string Contrasena);
public record RegistrarUsuarioResponse(Guid Id);

public record LoginUsuarioRequest(string CorreoElectronico, string Contrasena);
public record LoginUsuarioResponse(string Token);

public record ModificarMisDatosRequest(Guid Id, string Contrasena, string Nombre);

public record ListarUsuariosRequest(Guid Id);

public record UsuarioDTO(Guid Id, string Nombre, string CorreoElectronico, bool EsAdministrador, List<Permiso> Permisos);
public record ListarUsuariosResponse(List<UsuarioDTO> Usuarios);

public record EliminarUsuarioRequest(Guid IdOrigen, Guid IdAEliminar);

public record ModificarPermisosUsuarioRequest(Guid IdOrigen, Guid IdAEditar, Permiso NuevoPermiso, bool Asignar);