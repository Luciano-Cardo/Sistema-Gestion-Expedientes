using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class ModificarCaratulaExpedienteUseCase
{
    private IAutorizacionService _autorizacion;
    private IExpedienteRepository _expRepo;

    public ModificarCaratulaExpedienteUseCase(IExpedienteRepository expRepo, IAutorizacionService autorizacion)
    {
        _expRepo = expRepo;
        _autorizacion = autorizacion;
    }

    public ModificarCaratulaExpedienteResponse Ejecutar(ModificarCaratulaExpedienteRequest request)
    {
        if (!_autorizacion.PoseeElPermiso(request.Id, Permiso.ExpedienteModificacion)){
            throw new AutorizacionException("El usuario no posee la autorizacion");
        }

        var expediente = _expRepo.ObtenerPorId(request.Id);

        if (expediente == null)
        {
            throw new EntidadNoEncontradaException("El expediente solicitado no existe");
        }

        var caratula = new Caratula(request.NuevaCaratula);

        expediente.ModificarCaratula(caratula, request.Id);

        _expRepo.Modificar(expediente);

        return new ModificarCaratulaExpedienteResponse(request.Id, caratula , expediente.FechaUltimaModificacion);
    }
}