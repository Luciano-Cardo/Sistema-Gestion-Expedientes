using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public record AgregarTramiteRequest(Guid UsuarioUltimoCambio, Guid ExpedienteId, EtiquetaTramite Etiqueta, ContenidoTramite Contenido);
public record AgregarTramiteResponse(Guid Id, DateTime FechaCreacion);

public record EliminarTramiteRequest(Guid IdUsuario, Guid IdTramite);
public record EliminarTramiteResponse(Guid Id);

public record ModificarTramiteRequest(Guid IdTramite, ContenidoTramite NuevoContenido, Guid IdUsuario);
public record ModificarTramiteResponse(Guid Id, Guid UsuarioUltimoCambio, DateTime FechaUltimaModificacion);

public record ListarTramitesPorExpedienteRequest(Guid ExpedienteId);
public record TramiteResumenResponse(Guid Id, Guid ExpedienteId, EtiquetaTramite Etiqueta, ContenidoTramite Contenido, DateTime FechaCreacion);