using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public record AgregarExpedienteRequest(string Caratula, Guid IdUsuario);
public record AgregarExpedienteResponse(Guid Id);

public record EliminarExpedienteRequest(Guid Id, Guid IdUsuario);
public record EliminarExpedienteResponse(Guid Id);

public record ModificarCaratulaExpedienteRequest(Guid Id, string Caratula, Guid IdUsuario);
public record ModificarCaratulaExpedienteResponse(Guid Id, string Caratula, DateTime FechaUltimaModificacion);

public record CambiarEstadoExpedienteRequest(Guid Id, EstadoExpediente NuevoEstado, Guid IdUsuario);
public record CambiarEstadoExpedienteResponse(Guid Id, EstadoExpediente Estado, DateTime FechaUltimaModificacion);

public record ListarExpedientesRequest();
public record ExpedienteResumenResponse(Guid Id, string Caratula, EstadoExpediente Estado, DateTime FechaCreacion, DateTime FechaUltimaModificacion,Guid UsuarioUltimoCambio);