
using System.Data;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;


public record AgregarExpedienteRequest(String Caratula, Guid UsuarioUltimoCambio);
public record AgregarExpedienteResponse(Guid id);

public record EliminarExpedienteRequest(Guid Id, Guid UsuarioUltimoCambio);
public record EliminarExpedienteResponse(Guid Id);

public record ModificarCaratulaExpedienteRequest(Guid Id, String NuevaCaratula, Guid UsuarioUltimoCambio);
public record ModificarCaratulaExpedienteResponse(Guid Id, Caratula Caratula, DateTime FechaUltimaModificacion);

public record CambiarEstadoExpedienteRequest(Guid Id, EstadoExpediente NuevoEstado, Guid UsuarioUltimoCambio);
public record CambiarEstadoExpedienteResponse(Guid Id, EstadoExpediente Estado, DateTime FechaUltimaModificacion);

public record ListarExpedientesRequest();
public record ExpedienteResumenResponse(Guid Id, Caratula Caratula, EstadoExpediente Estado, DateTime FechaCreacion, DateTime FechaUltimaModificacion,Guid UsuarioUltimoCambio);

