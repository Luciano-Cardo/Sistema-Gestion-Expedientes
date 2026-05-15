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
        if (!_autorizacion.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion)){
            throw new AutorizacionException("El usuario no posee la autorizacion");
        }

        var expediente = _expRepo.ObtenerPorId(request.Id);

        if (expediente == null)
        {
            throw new EntidadNoEncontradaException("El expediente solicitado no existe");
        }

        var caratula = new Caratula(request.Caratula);

        expediente.ModificarCaratula(caratula, request.IdUsuario);

        _expRepo.Modificar(expediente);

        return new ModificarCaratulaExpedienteResponse(request.Id, request.Caratula, expediente.FechaUltimaModificacion);
    }
}
