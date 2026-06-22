<<<<<<< Updated upstream
using SGE.Aplicacion.Autorizacion;

public record RegistrarUsuarioRequest(String CorreoElectronico, String Nombre, String contrasena);
public record RegistrarUsuarioResponse(Guid Id);
=======
using SGE.Dominio.Entidades;
using SGE.Dominio.Autorizacion;
using System;
using System.Collections.Generic;
namespace SGE.Aplicacion.Usuarios;

public record RegistrarUsuarioRequest(string CorreoElectronico, string Nombre, string contrasena);
public record RegistrarUsuarioResponse(Guid Id);

public record LoginUsuarioRequest(string CorreoElectronico, string contrasena);
public record LoginUsuarioResponse(string Token);

public record ModificarMisDatosRequest(Guid Id, string contrasena, string Nombre);

public record ListarUsuariosRequest(Guid Id);
public record UsuarioDTO(Guid Id, string Nombre, string CorreoElectronico, bool esAdministrador, List<Permiso> Permisos);
public record ListarUsuariosResponse(List<UsuarioDTO> Usuarios);

public record EliminarUsuarioRequest(Guid IdOrigen, Guid IdAEliminar);

public record ModificarPermisosUsuarioRequest(Guid IdOrigen, Guid IdAEditar, Permiso nuevoPermiso, bool asignar);
>>>>>>> Stashed changes
