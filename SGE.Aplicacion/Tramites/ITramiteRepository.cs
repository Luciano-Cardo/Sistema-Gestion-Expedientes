using System;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Interfaces;

public interface ITramiteRepository
{
    public void Agregar(Tramite tramite)
    {
        
    }

    void Modificar(Guid idTramite, Tramite nuevoTramite);

    void Eliminar(Guid idTramite);


    Tramite? ObtenerPorID(Guid idTramite);


    List<Tramite> obtenerPorExpedienteId(Guid idExpediente);
}
