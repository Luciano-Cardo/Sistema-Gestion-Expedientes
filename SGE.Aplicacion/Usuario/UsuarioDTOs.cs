using SGE.Aplicacion.Autorizacion;

public record RegistrarUsuarioRequest(String CorreoElectronico, String Nombre, String contrasena);
public record RegistrarUsuarioResponse(Guid Id);