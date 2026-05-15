using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
namespace SGE.Aplicacion.Tramites;



public record AgregarTramiteRequest(Caratula Caratula, Guid UsuarioUltimoCambio,Guid ExpedienteId, EtiquetaTramite Etiqueta, ContenidoTramite Contenido);
public record AgregarTramiteResponse(Guid UsuarioUltimoCambio,DateTime FechaCreacion);

public record EliminarTramiteRequest(Guid id, Guid IdTramiteEliminar);
public record EliminarTramiteResponse(Guid Id);

public record ModificarTramiteRequest(Guid id,ContenidoTramite NuevoContenidoTramite);
public record ModificarTramiteResponse(Guid id,Guid UsuarioUltimoCambio, DateTime FechaUltimaModificacion);
