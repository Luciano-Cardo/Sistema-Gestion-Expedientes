using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Interfaces;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Servicios;

public class ActualizacionEstadoExpedienteService
{
    private readonly IExpedienteRepository _repoExpediente;
    private readonly ITramiteRepository _repoTramite;

    public ActualizacionEstadoExpedienteService(IExpedienteRepository repoExpediente, ITramiteRepository repoTramite)
    {
        _repoExpediente = repoExpediente;
        _repoTramite = repoTramite;
    }

    public void Ejecutar (Guid ExpedienteId, Guid IdUsuario)
    {
        var expediente = _repoExpediente.ObtenerPorId(ExpedienteId);
        if(expediente == null) 
            throw new EntidadNoEncontradaException("No existe un expediente con ese ID");

        Tramite? ultimo = null;
        foreach (var t in _repoTramite.ObtenerPorExpedienteId(ExpedienteId))
        {
            if (ultimo == null || t.FechaCreacion > ultimo.FechaCreacion)
                ultimo = t;
        }
        EtiquetaTramite? ultimaEtiqueta = ultimo?.Etiqueta;
 
        bool cambio = expediente.ActualizarEstado(ultimaEtiqueta, IdUsuario);
        if (cambio)
            _repoExpediente.Modificar(expediente);
    }
}
