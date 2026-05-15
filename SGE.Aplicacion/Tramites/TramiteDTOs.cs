using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
namespace SGE.Aplicacion.Tramites;



public record AgregarTramiteRequest(Caratula Caratula, Guid UsuarioUltimoCambio,Guid ExpedienteId, EtiquetaTramite Etiqueta, ContenidoTramite Contenido);
public record AgregarTramiteResponse(Guid UsuarioUltimoCambio,DateTime FechaCreacion);
