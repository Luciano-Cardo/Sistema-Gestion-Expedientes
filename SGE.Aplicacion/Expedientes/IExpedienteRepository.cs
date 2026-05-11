using System;
using System.ComponentModel;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Interfaces;

public interface IExpedienteRepository
{
    void Agregar(Expediente expediente);

    void Modificar(Guid idExpediente, Expediente nuevoExpediente);

    void Eliminar(Guid idExpediente);

    Expediente? ObtenerPorID(Guid idExpediente);
    
    List<Expediente> ObtenerTodos();
}
