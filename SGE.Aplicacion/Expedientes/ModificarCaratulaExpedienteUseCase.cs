using SGE.Aplicacion.Comun;
using SGE.Dominio.Autorizacion;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public class ModificarCaratulaExpedienteUseCase
{
    private readonly IAutorizacionService _autorizacion;
    private readonly IExpedienteRepository _expRepo;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    public ModificarCaratulaExpedienteUseCase(IExpedienteRepository expRepo, IAutorizacionService autorizacion, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _expRepo = expRepo;
        _autorizacion = autorizacion;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public ModificarCaratulaExpedienteResponse Ejecutar(ModificarCaratulaExpedienteRequest request)
    {

        if (!_autorizacion.PoseeElPermiso(request.UsuarioUltimoCambio, Permiso.ExpedienteModificacion))
            throw new AutorizacionException("El usuario no posee la autorizacion para modificar expedientes");

        var expediente = _expRepo.ObtenerPorId(request.Id);
        if (expediente == null)
            throw new EntidadNoEncontradaException("No existe un expediente con ese ID");

        var nuevaCaratula = new Caratula(request.NuevaCaratula);

        expediente.ModificarCaratula(nuevaCaratula, request.UsuarioUltimoCambio);

        _expRepo.Modificar(expediente);
        _unidadDeTrabajo.Guardar();
        return new ModificarCaratulaExpedienteResponse(expediente.Id, expediente.Caratula, expediente.FechaUltimaModificacion);
    }
}