using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Entidades;
<<<<<<< HEAD
using SGE.Dominio.Autorizacion;
using System;
using System.Collections.Generic;
=======

>>>>>>> 8011cb158bd6994018b455084fbc0d202c758687
namespace SGE.Aplicacion.Usuarios;

public record RegistrarUsuarioRequest(String CorreoElectronico, String Nombre, String contrasena);
public record RegistrarUsuarioResponse(Guid Id);

<<<<<<< HEAD
public record LoginUsuarioRequest(string CorreoElectronico, string contrasena);
public record LoginUsuarioResponse(string Token);

public record ModificarMisDatosRequest(Guid Id, string contrasena, string Nombre);

public record ListarUsuariosRequest(Guid Id);
public record UsuarioDTO(Guid Id, string Nombre, string CorreoElectronico, bool esAdministrador, List<Permiso> Permisos);
public record ListarUsuariosResponse(List<UsuarioDTO> Usuarios);

public record EliminarUsuarioRequest(Guid IdOrigen, Guid IdAEliminar);

public record ModificarPermisosUsuarioRequest(Guid IdOrigen, Guid IdAEditar,Permiso nuevoPermiso, bool asignar);

=======
public record LoginUsuarioRequest(String CorreoElectronico, String contrasena);
public record LoginUsuarioResponse(String Token);

public record ModificarMisDatosRequest(Guid Id, String contrasena, String Nombre);

public record ListarUsuariosRequest(Guid Id);
public record ListarUsuariosResponse(List<Usuario> Usuarios);

public record EliminarUsuarioRequest(Guid IdOrigen, Guid IdAEliminar);

public record ModificarPermisosUsuarioRequest(Guid IdOrigen, Guid IdAEditar, Permiso nuevoPermiso, bool asignar);
>>>>>>> 8011cb158bd6994018b455084fbc0d202c758687
