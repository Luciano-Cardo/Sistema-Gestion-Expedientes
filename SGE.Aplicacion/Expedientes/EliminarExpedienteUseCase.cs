using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Interfaces;

namespace SGE.Aplicacion.Expedientes;

public class EliminarExpedienteUseCase
{
    private IExpedienteRepository _expRepo;
    private ITramiteRepository _tramiteRepo;
    private IAutorizacionService _autorizacion;

    public EliminarExpedienteUseCase(IExpedienteRepository expRepo, ITramiteRepository tramiteRepo, IAutorizacionService autorizacion)
    {
        _expRepo = expRepo;
        _tramiteRepo = tramiteRepo;
        _autorizacion = autorizacion;
    }

    public EliminarExpedienteResponse Ejecutar(EliminarExpedienteRequest request)
    {
        if (!_autorizacion.PoseeElPermiso(request.Id, Permiso.ExpedienteBaja))
        {
            throw new AutorizacionException("El usuario no posee la autorizacion");
        }

        var tramites = _tramiteRepo.ObtenerPorExpedienteId(request.Id);
        foreach(var tramite in tramites)
        {
            _tramiteRepo.Eliminar(tramite.Id);
        }

        _expRepo.Eliminar(request.Id);

        return new EliminarExpedienteResponse(request.Id);
    }
}