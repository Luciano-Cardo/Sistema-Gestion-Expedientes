using SGE.Aplicacion.Expedientes;
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

    public void Ejecutar (Guid expedienteId, Guid id)
    {
        var expediente = _repoExpediente.ObtenerPorId(expedienteId);
        if(expediente == null) throw new Exception("Expediente no encontrado");

        var lista = _repoTramite.ObtenerPorExpedienteId(expedienteId);
        Tramite? ultimo = null;
        foreach(var t in lista)
        {
            if(ultimo == null || t.FechaCreacion > ultimo.FechaCreacion) ultimo = t;
        }

        if(ultimo != null)
        {
            expediente.ActualizarEstado(ultimo.Etiqueta,id);
        }

    }
}
