using SGE.Dominio.Autorizacion;
using SGE.Aplicacion.Servicios;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Comun;

namespace SGE.Aplicacion.Expedientes;

public class EliminarExpedienteUseCase
{
    private readonly IExpedienteRepository _expRepo;
    private readonly ITramiteRepository _tramiteRepo;
    private readonly IAutorizacionService _autorizacion;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    public EliminarExpedienteUseCase(IExpedienteRepository expRepo, ITramiteRepository tramiteRepo, IAutorizacionService autorizacion,IUnidadDeTrabajo unidadDeTrabajo)
    {
        _expRepo = expRepo;
        _tramiteRepo = tramiteRepo;
        _autorizacion = autorizacion;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public EliminarExpedienteResponse Ejecutar(EliminarExpedienteRequest request)
    {
        if (!_autorizacion.PoseeElPermiso(request.UsuarioUltimoCambio, Permiso.ExpedienteBaja))
            throw new AutorizacionException("El usuario no posee la autorizacion para eliminar expedientes");

        var expediente = _expRepo.ObtenerPorId(request.Id);
        if (expediente == null)
            throw new EntidadNoEncontradaException("No existe un expediente con ese ID");

        var tramites = _tramiteRepo.ObtenerPorExpedienteId(request.Id);
        foreach(var tramite in tramites)
            _tramiteRepo.Eliminar(tramite.Id);

        _expRepo.Eliminar(request.Id);
        _unidadDeTrabajo.GuardarCambios();
        return new EliminarExpedienteResponse(request.Id);
    }
}